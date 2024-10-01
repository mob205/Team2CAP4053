using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    [Tooltip("How long the level should last, in seconds")]
    [field: SerializeField] public float LevelDuration { get; private set; }

    [Tooltip("SO representing the number of revives used. Needs to be reset at the start of every level")]
    [SerializeField] ScriptableInt _numRevivesUsed;
    public float TimeRemaining { get; private set; }

    public int PlayersAlive { get; private set; }
    public int TotalPlayers { get; private set; }

    private void Start()
    {
        if(_numRevivesUsed)
        {
            _numRevivesUsed.Value = 0;
        }

        if(PlayerInputManager.instance)
        {
            PlayerInputManager.instance.onPlayerJoined += OnJoin;
            PlayerInputManager.instance.onPlayerLeft += OnLeave;
        }

        TimeRemaining = LevelDuration;
    }

    private void Destroy()
    {
        if(PlayerInputManager.instance)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnJoin;
            PlayerInputManager.instance.onPlayerLeft -= OnLeave;
        }
    }

    private void Update()
    {
        TickTimer(Time.deltaTime);
    }

    private void OnJoin(PlayerInput player)
    {
        var health = player.GetComponent<PlayerHealth>();
        if(health)
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
        if(PlayersAlive <= 0)
        {
            // Defeat stuff here
        }
    }

    private void TickTimer(float deltaTime)
    {
        TimeRemaining -= deltaTime;
        if(TimeRemaining <= 0)
        {
            // Time victory stuff here
        }
    }
}
