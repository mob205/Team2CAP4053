using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour, IInteractable
{
    [field: Header("Gameplay")]
    [field: SerializeField] public ToolType RequiredTool { get; private set; }

    [Tooltip("Amount of time needed to interact with a breaking/broken spawner to repair it, in seconds")]
    [field: SerializeField] public float MaxRepairDuration { get; private set; } = 4f;

    [Tooltip("A SO to share the number of floorboards currently broken")]
    [SerializeField] private ScriptableInt _counter;

    [Tooltip("The maximum number of spawners of that share the counter SO that can be breaking/broken at a time")]
    [SerializeField] private int _maxBroken = 3;

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


    public float CurrentRepairTimer { get; private set; }   // Time left to repair broken item
    public float CurrentSpawnTimer { get; private set; }   // Time left for a broken item to spawn an enemy
    public float CurrentBreakingTimer { get; private set; }   // Time left for this item to break once it starts breaking
    public float BreakCheckTimer { get; private set; }   // Time left for this item to potentially start breaking, if repaired
    public bool IsRepairing { get; private set; }

    private List<GameObject> _players = new List<GameObject>();

    private PlayerInteractor _interactor;
    private AudioSource _audioSource;
    private State _currentState;
    private float _timeSinceLastBroken;
    
   

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
        _players.Add(playerInput.gameObject);
    }

    void OnPlayerLeave(PlayerInput playerInput)
    {
        _players.Remove(playerInput.gameObject);
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
            IsRepairing = true;
            CurrentRepairTimer = MaxRepairDuration;
            BreakCheckTimer = _repairGracePeriod;
            _interactor = player;
        }
    }

    public void StopInteract(PlayerInteractor player)
    {
        if (_interactor == player)
        {
            Debug.Log("Stopped repairing!");
            IsRepairing = false;
            CurrentRepairTimer = MaxRepairDuration;
            _interactor = null;
        }
    }

    public void UpdateRepair()
    {
        if (!IsRepairing) { return; }
        
        CurrentRepairTimer -= Time.deltaTime;
        if (CurrentRepairTimer < 0)
        {
            _currentState = State.Repaired;
            IsRepairing = false;

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
        if(_currentState == State.Broken && !IsRepairing)
        {
            CurrentSpawnTimer -= Time.deltaTime;
            if(CurrentSpawnTimer <= 0)
            {
                Debug.Log($"Spawning monster at {name}");
                CurrentSpawnTimer = Random.Range(_spawnDelayMinimum, _spawnDelayMaximum);
            }
        }

        // Change from breaking to broken
        if (_currentState == State.Breaking)
        {
            CurrentBreakingTimer -= Time.deltaTime;
            if(CurrentBreakingTimer < 0)
            {
                _currentState = State.Broken;
                Debug.Log($"Broken at {name}");
                CurrentSpawnTimer = Random.Range(_spawnDelayMinimum, _spawnDelayMaximum);

                if (_audioSource && _completeBreakSound)
                {
                    _completeBreakSound.Play(_audioSource);
                }
            }
        }
        // Change from repaired to breaking
        if (_currentState == State.Repaired && CanSpawn())
        {
            Debug.Log($"Breaking at {name}");
            _currentState = State.Breaking;
            _timeSinceLastBroken = 0;
            CurrentBreakingTimer = _breakingDuration;
            ++_counter.Value;
        }
    }

    private bool CanSpawn()
    {
        if (BreakCheckTimer > 0)
        {
            BreakCheckTimer -= Time.deltaTime;
            return false;
        }

        BreakCheckTimer = 1f / _breakChecksPerSecond;

        float distAvg = 0;
        if (_players.Count > 0)
        {
            float distSum = 0;
            foreach (var player in _players)
            {
                distSum += (transform.position - player.transform.position).sqrMagnitude;
            }
            distAvg = distSum / _players.Count;
            distAvg /= 1000;
        }
        
        float timeChance = _timeSinceLastBroken * (_baseBreakChancePerMinute / 60);
        float distChance = distAvg * _breakChanceDistanceFactor;
        float totalChance = (_maxBroken - _counter.Value) * (timeChance + distChance);
        return (Random.Range(0f, 100f) < totalChance);
    }

    private bool HasCorrectTool(PlayerInteractor player)
    {
        return (player.HeldTool == null && RequiredTool == null) || (player.HeldTool != null && player.HeldTool.ToolType == RequiredTool);
    }
}
