using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : DurationInteractable
{
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
    [Tooltip("Sound to be played after a successful repair")]
    [SerializeField] private AudioEvent _completeRepairSound;

    [Tooltip("Sound to be played after the spawner breaks")]
    [SerializeField] private AudioEvent _completeBreakSound;

    enum State
    { 
        Repaired,
        Breaking,
        Broken,
    }

    public float CurrentSpawnTimer { get; private set; }   // Time left for a broken item to spawn an enemy
    public float CurrentBreakingTimer { get; private set; }   // Time left for this item to break once it starts breaking
    public float BreakCheckTimer { get; private set; }   // Time left for this item to potentially start breaking, if repaired
    public bool IsRepairing { get; private set; }

    private List<GameObject> _players = new List<GameObject>();

    private AudioSource _audioSource;
    private State _currentState;
    private float _timeSinceLastBroken;
    
   

    protected override void Update()
    {
        base.Update();
        if (_currentState == State.Repaired)
        {
            _timeSinceLastBroken += Time.deltaTime;
        }
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

    public override bool IsInteractable(ToolType tool)
    {
        return _currentState != State.Repaired && base.IsInteractable(tool);
    }

    protected override void CompleteInteraction()
    {
        _currentState = State.Repaired;
        --_counter.Value;
        BreakCheckTimer = _repairGracePeriod;

        if(_audioSource && _completeRepairSound)
        {
            _completeRepairSound.Play(_audioSource);
        }
    }

    protected override float GetInteractionDuration()
    {
        return MaxRepairDuration;
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
                // Spawn monster here
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

                // Switch to broken model

                CurrentSpawnTimer = Random.Range(_spawnDelayMinimum, _spawnDelayMaximum);

                if (_audioSource && _completeBreakSound)
                {
                    _completeBreakSound.Play(_audioSource);
                }
            }
        }
        // Change from repaired to breaking
        if (_currentState == State.Repaired && CanBreak())
        {
            // Switch model/play animation/etc.

            _currentState = State.Breaking;
            _timeSinceLastBroken = 0;
            CurrentBreakingTimer = _breakingDuration;
            ++_counter.Value;
        }
    }

    private bool CanBreak()
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

    
}
