using UnityEngine;

public class WindupEnemySpawner : DurationInteractable
{
    [field: Tooltip("Amount of time needed to interact with this spawner to wind it up, in seconds")]
    [field: SerializeField] public float MaxWindupDuration { get; private set; } = 4f;

    [field: Tooltip("Amount of time needed for this spawner to fully wind down and spawn an enemy, in seconds")]
    [field: SerializeField] public float MaxWindDownDuration { get; private set; } = 20f;

    [field: Tooltip("Amount of time for the spawner to re-wind itself up after being wound down, in seconds")]
    [field: SerializeField] private float _selfWindupDelay = 10f;

    [SerializeField] private Enemy _spawnedEnemyObj;

    [Tooltip("Offset from enemy spawner to spawn enemies from")]
    [SerializeField] private Vector3 _spawnOffset;
    public float WindupRatio { get { return 1 - Mathf.Clamp01(_windupRemaining / MaxWindDownDuration); } }

    float _windupRemaining;
    bool _hasSpawned = false;

    private void Start()
    {
        _windupRemaining = MaxWindDownDuration;
    }

    protected override void Update()
    {
        _windupRemaining -= Time.deltaTime;
        base.Update();

        if (_windupRemaining < 0 && !IsInProgress && !_hasSpawned)
        {
            _hasSpawned = true;
            SpawnEnemy();
        }

        if(_windupRemaining < -_selfWindupDelay)
        {
            CompleteInteraction();
        }
    }

    private void SpawnEnemy()
    {
        Debug.Log("Spawn enemy");
        var enemy = Instantiate(_spawnedEnemyObj, transform.position + _spawnOffset, transform.rotation);
        GameStateManager.Instance.RegisterEnemy(enemy);
    }
    protected override void CompleteInteraction()
    {
        _windupRemaining = MaxWindDownDuration;
        _hasSpawned = false;
    }

    protected override float GetInteractionDuration()
    {
        return MaxWindupDuration;
    }
    public override bool IsInteractable(ToolType tool)
    {
        return base.IsInteractable(tool) && !_hasSpawned;
    }
}
