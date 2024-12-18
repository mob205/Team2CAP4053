using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class EnemySpawner : DurationInteractable
{
    [Header("Repairing")]
    [Tooltip("Repair durations for each player count")]
    [SerializeField] private float[] _repairDurations = new float[4];

    [Tooltip("Amount of time needed to interact with a breaking/broken spawner to repair it, in seconds")]
    public float MaxRepairDuration { get; private set; } = 4f;

    [Tooltip("Amount of time after repairing boards where they will not perform a break check")]
    [SerializeField] private float _repairGracePeriod = 5f;


    [Header("Spawning")]
    [SerializeField] private Enemy _spawnedEnemyObj;

    [Tooltip("Minimum amount of time it takes for a monster to spawn, in seconds")]
    [SerializeField] private float[] _spawnDelayMinimums = new float[4];
    private float _spawnDelayMinimum = 999;


    [Tooltip("Maximum amount of time it takes for a monster to spawn, in seconds")]
    [SerializeField] private float[] _spawnDelayMaximums = new float[4];
    private float _spawnDelayMaximum = 999;

    [Tooltip("Where to spawn enemies from")]
    [SerializeField] private Transform _spawnPoint;

#if UNITY_EDITOR
    [SerializeField] private bool _debugAllowSpawning;
#endif

    [Header("Breaking")]

    [Tooltip("A SO to share the number of floorboards currently broken")]
    [SerializeField] private ScriptableInt _counter;

    [Tooltip("The maximum number of spawners of that share the counter SO that can be breaking/broken at a time")]
    [SerializeField] private int _maxBroken = 3;

    [Tooltip("Amount of time needed for this spawner to fully break, in second.")]
    [SerializeField] private float[] _breakingDurations = new float[4];
    private float _breakingDuration = 5f;

    [Tooltip("Rate at which the likelihood of this spawner begins breaking increases. Likelihood is % chance of it happening any given check")]
    [SerializeField] private float[] _baseBreakChancesPerMinute = new float[4];
    private float _baseBreakChancePerMinute = 0;

    [Tooltip("Factor to increase the spawner's likelihood to break with average distance from all players")]
    [SerializeField] private float _breakChanceDistanceFactor = 0;

    [Tooltip("The number of times a check for this spawner to break happens every second")]
    [SerializeField] private int _breakChecksPerSecond = 4;

    private float _breakChanceModifier = 1;

    public UnityEvent OnStartBreaking;
    public UnityEvent OnBreak;
    public UnityEvent OnRepair;

    enum State
    { 
        Repaired,
        Breaking,
        Broken,
    }

    public float CurrentSpawnTimer      { get; private set; }   // Time left for a broken item to spawn an enemy
    public float CurrentBreakingTimer   { get; private set; }   // Time left for this item to break once it starts breaking
    public float BreakCheckTimer        { get; private set; }   // Time left for this item to potentially start breaking, if repaired

    private State _currentState;
    private float _timeSinceLastBroken;

    protected override void Update()
    {
        base.Update();
        if (_currentState == State.Repaired)
        {
            _timeSinceLastBroken += Time.deltaTime * (1 - (_counter.Value / _maxBroken));
        }
        UpdateBreaking();
    }

    protected override void Start()
    {
        base.Start();
        _counter.Value = 0;
    }
    public override bool IsInteractable(ToolType tool)
    {
        return _currentState != State.Repaired && base.IsInteractable(tool);
    }
    protected override void UpdateStatsByPlayer(int playerCount)
    {
        base.UpdateStatsByPlayer(playerCount);
        playerCount = Mathf.Min(playerCount - 1, 3);

        MaxRepairDuration = _repairDurations[playerCount];
        _spawnDelayMinimum = _spawnDelayMinimums[playerCount];
        _spawnDelayMaximum = _spawnDelayMaximums[playerCount];
        _breakingDuration = _breakingDurations[playerCount];
        _baseBreakChancePerMinute = _baseBreakChancesPerMinute[playerCount];
    }
    protected override void CompleteInteraction()
    {
        _currentState = State.Repaired;
        --_counter.Value;
        BreakCheckTimer = _repairGracePeriod;

        OnRepair?.Invoke();
    }

    protected override float GetInteractionDuration()
    {
        return MaxRepairDuration;
    }

    private void UpdateBreaking()
    {
        // Spawns monsters if broken
        // Don't spawn monsters if player is actively repairing
        if (_currentState == State.Broken && !IsInProgress)
        {
            CurrentSpawnTimer -= Time.deltaTime;
            if(CurrentSpawnTimer <= 0)
            {

#if UNITY_EDITOR
                if (!_debugAllowSpawning) { return; }
#endif

                var enemy = Instantiate(_spawnedEnemyObj, _spawnPoint.position, _spawnPoint.rotation);
                GameStateManager.Instance.RegisterEnemy(enemy);

                CurrentSpawnTimer = Random.Range(_spawnDelayMinimum, _spawnDelayMaximum);
            }
        }

        // Change from breaking to broken. Don't break if actively repairing
        if (_currentState == State.Breaking && !IsInProgress)
        {
            CurrentBreakingTimer -= Time.deltaTime;
            if(CurrentBreakingTimer < 0)
            {
                _currentState = State.Broken;

                CurrentSpawnTimer = Random.Range(_spawnDelayMinimum, _spawnDelayMaximum);

                OnBreak?.Invoke();
            }
        }
        if (_currentState == State.Repaired && CanBreak())
        {
            _currentState = State.Breaking;
            _timeSinceLastBroken = 0;
            CurrentBreakingTimer = _breakingDuration;
            ++_counter.Value;

            OnStartBreaking?.Invoke();
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

        var players = GameStateManager.Instance.Players;

        if (players.Count > 0)
        {
            float distSum = 0;
            foreach (var player in players)
            {
                distSum += (transform.position - player.transform.position).sqrMagnitude;
            }
            distAvg = distSum / players.Count;
            distAvg /= 1000;
        }
        
        float timeChance = _timeSinceLastBroken * (_baseBreakChancePerMinute / 60);
        float distChance = distAvg * _breakChanceDistanceFactor;
        float totalChance = _breakChanceModifier * (_maxBroken - _counter.Value) * (timeChance + distChance);
        return (Random.Range(0f, 100f) < totalChance);
    }

    public void AddBreakChanceModifier(float modifier)
    {
        _breakChanceModifier += modifier;
    }
}
