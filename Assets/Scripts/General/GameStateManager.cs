using UnityEngine.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [Tooltip("How long the level should last, in seconds")]
    [field: SerializeField] public float LevelDuration { get; private set; }

    [Tooltip("SO representing the number of revives used. Provide this so it can be reset at the start of every level")]
    [SerializeField] ScriptableInt _numRevivesUsed;

    public UnityEvent OnGameWin;
    public UnityEvent OnGameLose;
    public UnityEvent OnGameEnd;
    public UnityEvent OnGameStart;
    
    public static GameStateManager Instance { get; private set; }
    public List<PlayerInput> Players { get; private set; } = new();

    public float TimeRemaining { get; private set; }
    public int PlayersAlive { get; private set; }
    public int TotalPlayers { get; private set; }

    public bool HasHighscore { get; private set; }

    private bool _isGameActive;

    private List<Enemy> _spawnedEnemies = new();

    public void RegisterEnemy(Enemy enemy)
    {
        _spawnedEnemies.Add(enemy);
        enemy.OnKilled += RemoveEnemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    private void Start()
    {
        if (_numRevivesUsed)
        {
            _numRevivesUsed.Value = 0;
        }
        TimeRemaining = LevelDuration;
    }


    private void Update()
    {
        if (_isGameActive)
        {
            TickTimer(Time.deltaTime);
        }
    }

    public void StartGame()
    {
        _isGameActive = true;
        OnGameStart?.Invoke();
    }

    public void OnJoin(PlayerInput player)
    {
        if (player.TryGetComponent(out PlayerHealth health))
        {
            health.OnDeath.AddListener(OnDeath);
            health.OnRevive.AddListener(OnRevive);
        }
        else
        {
            Debug.LogError("No PlayerHealth found on this player object.");
        }

        Players.Add(player);

        ++PlayersAlive;
        ++TotalPlayers;
    }
    private void OnLeave(PlayerInput player)
    {
        --PlayersAlive;
        --TotalPlayers;
        CheckAliveStatus();
        Players.Remove(player);
    }

    private void OnDeath(PlayerHealth player)
    {
        --PlayersAlive;
        CheckAliveStatus();
    }

    private void OnRevive(PlayerHealth player)
    {
        ++PlayersAlive;
    }
    private void CheckAliveStatus()
    {
        if (PlayersAlive <= 0 && _isGameActive)
        {
            EndGame();
            OnGameLose?.Invoke();
        }
    }
    private void EndGame()
    {
        _isGameActive = false;
        KillAllEnemies();
        OnGameEnd?.Invoke();
    }

    private void KillAllEnemies()
    {
        foreach(var enemy in _spawnedEnemies)
        {
            enemy.OnKilled -= RemoveEnemy;
            enemy.Kill();
        }
        _spawnedEnemies.Clear();
    }

    private void TickTimer(float deltaTime)
    {
        TimeRemaining -= deltaTime;
        if (TimeRemaining <= 0 && _isGameActive)
        {
            EndGame();

            if (TryGetComponent(out PointCalculator points))
            {
                HasHighscore = LevelManager.TrySetHighscore(SceneManager.GetActiveScene().name, points.Points);
            }

            OnGameWin?.Invoke();
        }
    }
}
