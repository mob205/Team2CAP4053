using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour, IInteractable
{
    [field: Header("Gameplay")]
    public ToolType RequiredTool { get; private set; }

    [Tooltip("A SO to share the number of floorboards currently broken")]
    [SerializeField] private ScriptableInt _counter;

    [Tooltip("The maximum number of spawners of that share the counter SO that can be breaking/broken at a time")]
    [SerializeField] private int _maxBroken = 3;

    [Tooltip("Amount of time needed to interact with a breaking/broken spawner to repair it, in seconds")]
    [SerializeField] private float _repairDuration = 4f;

    [Tooltip("Amount of time needed for this spawner to fully break, in second.")]
    [SerializeField] private float _breakingDuration = 5f;

    [Tooltip("Amount of time after repairing boards where they will not perform a break check")]
    [SerializeField] private float _repairGracePeriod = 5f;

    [Tooltip("Minimum amount of time it takes for a monster to spawn, in seconds")]
    [SerializeField] private float _spawnDelayMinimum;

    [Tooltip("Maximum amount of time it takes for a monster to spawn, in seconds")]
    [SerializeField] private float _spawnDelayMaximum;

    [Tooltip("Rate at which the likelihood of this spawner begins breaking increases. Likelihood is % chance of it happening any given check")]
    [SerializeField] private float _baseBreakChancePerMinute;

    [Tooltip("Factor to increase the spawner's likelihood to break with average distance from all players")]
    [SerializeField]
    private float _breakChanceDistanceFactor;

    [Tooltip("The number of times a check for this spawner to break happens every second")]
    [SerializeField] private int _breakChecksPerSecond = 4;

    [Header("Audio")]
    [SerializeField] private AudioEvent _completeRepairSound;
    [SerializeField] private AudioEvent _completeBreakSound;

    enum State
    { 
        Repaired,
        Breaking,
        Broken,
    }

    List<GameObject> players = new List<GameObject>();

    private PlayerInteractor _interactor;
    private AudioSource _audioSource;

    private State _currentState;
    private bool _isRepairing;

    private float _timeSinceLastBroken;
    
    private float _repairTimer;         // Time left to repair broken item
    private float _spawnTimer;          // Time left for a broken item to spawn an enemy
    private float _breakingTimer;       // Time left for this item to break once it starts breaking
    private float _breakCheckTimer;     // Time left for this item to potentially start breaking, if repaired

    private void Update()
    {
        if (_currentState == State.Repaired)
        {
            _timeSinceLastBroken += Time.deltaTime;
        }
        UpdateRepair();
        UpdateBreaking();
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _counter.Value = 0;
        if(PlayerInputManager.instance)
        {
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoin;
            PlayerInputManager.instance.onPlayerLeft += OnPlayerLeave;
        }
    }

    private void OnDestroy()
    {
        if(PlayerInputManager.instance)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoin;
            PlayerInputManager.instance.onPlayerLeft -= OnPlayerLeave;
        }
    }

    void OnPlayerJoin(PlayerInput playerInput)
    {
        Debug.Log("Spawned");
        players.Add(playerInput.gameObject);
    }

    void OnPlayerLeave(PlayerInput playerInput)
    {
        players.Remove(playerInput.gameObject);
    }

    public bool IsInteractable()
    {
        return _currentState != State.Repaired && _interactor == null;
    }

    public void StartInteract(PlayerInteractor player)
    {
        if(_interactor == null && HasCorrectTool(player))
        {
            Debug.Log("Repairing...");
            _isRepairing = true;
            _repairTimer = _repairDuration;
            _breakCheckTimer = _repairGracePeriod;
            _interactor = player;
        }
    }

    public void StopInteract(PlayerInteractor player)
    {
        if (_interactor == player)
        {
            Debug.Log("Stopped repairing!");
            _isRepairing = false;
            _repairTimer = _repairDuration;
            _interactor = null;
        }
    }

    public void UpdateRepair()
    {
        if (!_isRepairing) { return; }
        
        _repairTimer -= Time.deltaTime;
        if (_repairTimer < 0)
        {
            _currentState = State.Repaired;
            _isRepairing = false;

            --_counter.Value;

            if (_audioSource && _completeBreakSound)
            {
                _completeRepairSound.Play(_audioSource);
            }
        }
    }

    public void UpdateBreaking()
    {
        // Spawns monsters if broken
        // Don't spawn monsters if player is actively repairing
        if(_currentState == State.Broken && !_isRepairing)
        {
            _spawnTimer -= Time.deltaTime;
            if(_spawnTimer <= 0)
            {
                Debug.Log("Spawning monster!");
                _spawnTimer = Random.Range(_spawnDelayMinimum, _spawnDelayMaximum);
            }
        }

        // Change from breaking to broken
        if (_currentState == State.Breaking)
        {
            _breakingTimer -= Time.deltaTime;
            if(_breakingTimer < 0)
            {
                _currentState = State.Broken;
                Debug.Log("Broken!");
                _spawnTimer = Random.Range(_spawnDelayMinimum, _spawnDelayMaximum);

                if (_audioSource && _completeBreakSound)
                {
                    _completeBreakSound.Play(_audioSource);
                }
            }
        }
        // Change from repaired to breaking
        if (_currentState == State.Repaired && CanSpawn())
        {
            Debug.Log("Breaking!!!");
            _currentState = State.Breaking;
            _timeSinceLastBroken = 0;
            _breakingTimer = _breakingDuration;
            ++_counter.Value;

        }
    }

    private bool CanSpawn()
    {
        if (_breakCheckTimer > 0)
        {
            _breakCheckTimer -= Time.deltaTime;
            return false;
        }

        _breakCheckTimer = 1f / _breakChecksPerSecond;

        float distAvg = 0;
        if (players.Count > 0)
        {
            float distSum = 0;
            foreach (var player in players)
            {
                distSum += (transform.position - player.transform.position).sqrMagnitude;
            }
            distAvg = distSum / players.Count;
            Debug.Log(distAvg);
            distAvg /= 1000;
        }
        
        float timeChance = _timeSinceLastBroken * (_baseBreakChancePerMinute / 60);
        float distChance = distAvg * _breakChanceDistanceFactor;
        float totalChance = (_maxBroken - _counter.Value) * (timeChance + distChance);
        Debug.Log($"Time chance: {timeChance} | Dist chance: {distChance} | Total chance: {totalChance}");
        return (Random.Range(0f, 100f) < totalChance);
    }

    private bool HasCorrectTool(PlayerInteractor player)
    {
        return (player.HeldTool == null && RequiredTool == null) || (player.HeldTool != null && player.HeldTool.ToolType == RequiredTool);
    }
}
