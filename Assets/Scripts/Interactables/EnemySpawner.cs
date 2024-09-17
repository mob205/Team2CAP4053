using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IInteractable
{
    public ToolType RequiredTool { get; private set; }

    [Tooltip("Amount of time needed to interact with an item to repair it, in seconds")]
    [SerializeField] float _repairDuration = 4f;

    [Tooltip("Amount of time needed for this item to fully break, in second.")]
    [SerializeField] float _breakingDuration = 5f;

    [Tooltip("Minimum amount of time it takes for a monster to spawn, in seconds")]
    [SerializeField] float _spawnDelayMinimum;

    [Tooltip("Maximum amount of time it takes for a monster to spawn, in seconds")]
    [SerializeField] float _spawnDelayMaximum;

    [Tooltip("Rate at which the likelihood of this item begins breaking increases. Likelihood is % chance of it happening any given check")]
    [SerializeField] double _breakChancePerMinute;

    [Tooltip("The number of times a check for an item to break should happen every second")]
    [SerializeField] int _breakChecksPerSecond = 4;

    //[Tooltip("The enemy that will periodically spawn when broken")]
    //[SerializeField] GameObject _enemy;

    enum State
    { 
        Repaired,
        Breaking,
        Broken,
    }

    PlayerInteractor _interactor;

    private State _currentState;
    private bool _isRepairing;

    private float _timeSinceLastBroken;
    
    private float _repairTimer;         // Time left to repair broken item
    private float _spawnTimer;          // Time left for a broken item to spawn an enemy
    private float _breakingTimer;       // Time left for this item to break once it starts breaking
    private float _breakCheckTimer;     // Time left for this item to potentially start breaking, if repaired

    public bool IsInteractable()
    {
        return _currentState != State.Repaired;
    }

    public void StartInteract(PlayerInteractor player)
    {
        if(_interactor == null && HasCorrectTool(player))
        {
            Debug.Log("Repairing...");
            _isRepairing = true;
            _repairTimer = _repairDuration;
        }
        
    }

    public void StopInteract(PlayerInteractor player)
    {
        if(_interactor != null)
        {
            Debug.Log("Stopped repairing!");
            _isRepairing = false;
            _repairTimer = _repairDuration;
            _interactor = null;
        }
    }

    public void UpdateRepair()
    {
        if (_isRepairing)
        {
            _repairTimer -= Time.deltaTime;
            if (_repairTimer < 0)
            {
                _currentState = State.Repaired;
                _isRepairing = false;
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
                Debug.Log("Spawning monster!");
                _spawnTimer = Random.Range(_spawnDelayMinimum, _spawnDelayMaximum);
            }
        }
        // Change from repaired to breaking
        if (_currentState == State.Repaired && CanSpawn())
        {
            Debug.Log("Breaking!!!");
            _currentState = State.Breaking;
            _timeSinceLastBroken = 0;
            _breakingTimer = _breakingDuration;
        }
    }

    private bool CanSpawn()
    {
        if(_breakCheckTimer > 0)
        {
            _breakCheckTimer -= Time.deltaTime;
            return false;
        }
        _breakCheckTimer = 1f / _breakChecksPerSecond;
        return (Random.Range(0f, 100f) < _timeSinceLastBroken * (_breakChancePerMinute / 60));
    }
    private bool HasCorrectTool(PlayerInteractor player)
    {
        return (player.HeldTool == null && RequiredTool == null) || (player.HeldTool != null && player.HeldTool.ToolType == RequiredTool);
    }

    void Update()
    {
        if (_currentState == State.Repaired)
        {
            _timeSinceLastBroken += Time.deltaTime;
        }
        UpdateRepair();
        UpdateBreaking();
    }
}
