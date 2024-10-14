using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointCalculator : MonoBehaviour
{

    [Tooltip("Base amount of points to be awarded per second")]
    [SerializeField] private int _basePointsPerSecond;

    [Tooltip("Penalty applied to the base point gain for each spawner broken")]
    [SerializeField] private int _decreasePerBreak;

    [Tooltip("Amount of points to be deducted when a spawner breaks")]
    [SerializeField] private int _spawnerBreakPenalty;

    [Tooltip("Amount of points to be deducted when a player dies")]
    [SerializeField] private int _playerDeathPenalty;

    public float Points { get; private set; }

    private bool _canAddPoints;

    private int _numSpawnersBroken;

    private void Awake()
    {
        var spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
        foreach(var spawner in spawners)
        {
            spawner.OnBreak.AddListener(OnSpawnerBreak);
            spawner.OnRepair.AddListener(OnSpawnerRepair);
        }
    }
    private void Start()
    {
        ReadyUpManager.Instance.OnReadyUp.AddListener(OnReadyUp);
    }

    private void Update()
    {
        if(_canAddPoints)
        {
            Points += (_basePointsPerSecond - (_decreasePerBreak * _numSpawnersBroken)) * Time.deltaTime;
        }

        Debug.Log(Points);
    }

    private void OnReadyUp()
    {
        var players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);
        foreach (var player in players)
        {
            player.OnDeath.AddListener(OnPlayerDeath);
        }

        _canAddPoints = true;
    }

    private void OnSpawnerBreak()
    {
        Points -= _spawnerBreakPenalty;
        ++_numSpawnersBroken;
    }

    private void OnSpawnerRepair()
    {
        _numSpawnersBroken = Mathf.Max(0, _numSpawnersBroken - 1);
    }

    private void OnPlayerDeath(PlayerHealth health)
    {
        Points -= _playerDeathPenalty;
    }
}
