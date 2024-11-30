using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class OracleSpawner : MonoBehaviour
{
    [SerializeField] private Enemy _oraclePrefab;

    [Tooltip("Maximum number of oracles that can spawn")]
    [SerializeField] private int _maxSpawns;

    [Tooltip("Minimum duration for an oracle to spawn")]
    [SerializeField] private float _minSpawnDuration;

    [Tooltip("Maximum duration for an oracle to spawn")]
    [SerializeField] private float _maxSpawnDuration;

    [Tooltip("Locations where an oracle can spawn")]
    [SerializeField] private Transform[] _spawnLocations;

    [SerializeField] private Vector3 _spawnLocationOffset;

    private bool[] _hasActiveOracle;


    private float _currentSpawnDelay;

    private bool _isActive = false;
    private int _numCurSpawned = 0;

    private void Awake()
    {
        if(_spawnLocations.Length == 0)
        {
            Debug.LogError("No oracle spawn locations specified!");
            Destroy(this);
        }
        if(_spawnLocations.Length < _maxSpawns)
        {
            Debug.LogWarning("There are less oracle spawn locations than number of oracles that can spawn.");
        }
        _hasActiveOracle = new bool[_spawnLocations.Length];
        _currentSpawnDelay = Random.Range(_minSpawnDuration, _maxSpawnDuration);
    }
    private void Start()
    {
        GameStateManager.Instance.OnGameStart.AddListener(() => _isActive = true);
        GameStateManager.Instance.OnGameEnd.AddListener(() => _isActive = false);
    }

    private void Update()
    {
        if(!_isActive || _numCurSpawned == _maxSpawns) { return; }

        _currentSpawnDelay -= Time.deltaTime;
        if(_currentSpawnDelay < 0)
        {
            SpawnOracle();
            _currentSpawnDelay = Random.Range(_minSpawnDuration, _maxSpawnDuration);
        }
    }

    private void SpawnOracle()
    {
        int spawnIndex = GetUnusedSpawnIndex();
        _hasActiveOracle[spawnIndex] = true;
        _numCurSpawned++;

        var oracle = Instantiate(_oraclePrefab, _spawnLocations[spawnIndex].position + _spawnLocationOffset, Quaternion.identity);
        oracle.OnKilled += (Enemy) => OnOracleDeath(spawnIndex);

        GameStateManager.Instance.RegisterEnemy(oracle);
    }

    private void OnOracleDeath(int spawnIndex)
    {
        _hasActiveOracle[spawnIndex] = false;
        _numCurSpawned--;
    }

    private int GetUnusedSpawnIndex()
    {
        for(int i = 0; i < _spawnLocations.Length; i++)
        {
            if (!_hasActiveOracle[i])
            {
                return i;
            }
        }
        return 0;
    }
}

