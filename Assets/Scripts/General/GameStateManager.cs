using UnityEngine.Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    [Tooltip("How long the level should last, in seconds")]
    [field: SerializeField] public float LevelDuration { get; private set; }

    [Tooltip("SO representing the number of revives used. Provide this so it can be reset at the start of every level")]
    [SerializeField] ScriptableInt _numRevivesUsed;

    public UnityEvent OnGameWin;
    public UnityEvent OnGameLose;
    public UnityEvent OnGameEnd;

    public static GameStateManager Instance { get; private set; }

    public float TimeRemaining { get; private set; }
    public int PlayersAlive { get; private set; }
    public int TotalPlayers { get; private set; }

    private bool _isTicking;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    private void Start()
    {
        if(_numRevivesUsed)
        {
            _numRevivesUsed.Value = 0;
        }
        TimeRemaining = LevelDuration;
    }


    private void Update()
    {
        if(_isTicking)
        {
            TickTimer(Time.deltaTime);
        }
    }

    public void StartTicking()
    {
        _isTicking = true;
    }

    public void OnJoin(PlayerInput player)
    {
        if(player.TryGetComponent(out PlayerHealth health))
        {
            health.OnDeath.AddListener(OnDeath);
            health.OnRevive.AddListener(OnRevive);
        }
        else
        {
            Debug.LogError("No PlayerHealth found on this player object.");
        }

        ++PlayersAlive;
        ++TotalPlayers;
    }
    private void OnLeave(PlayerInput player)
    {
        --PlayersAlive;
        --TotalPlayers;
        CheckAliveStatus();
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
        Debug.Log(PlayersAlive);
        if (PlayersAlive <= 0)
        {
            EndGame();
            OnGameLose?.Invoke();
        }
    }
    
    private void EndGame()
    {
        _isTicking = false;
        OnGameEnd?.Invoke();
    }

    private void TickTimer(float deltaTime)
    {
        TimeRemaining -= deltaTime;
        if(TimeRemaining <= 0)
        {
            EndGame();
            OnGameWin?.Invoke();
        }
    }
}