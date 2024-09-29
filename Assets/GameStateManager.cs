using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    [field: SerializeField] public float LevelDuration { get; private set; }
    public float TimeRemaining { get; private set; }

    public int PlayersAlive { get; private set; }
    public int TotalPlayers { get; private set; }

    private void Start()
    {
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

    private void CheckAliveStatus()
    {
        if(PlayersAlive <= 0)
        {
            Debug.Log("DEFEAT!");
        }
    }

    private void TickTimer(float deltaTime)
    {
        TimeRemaining -= deltaTime;
        if(TimeRemaining <= 0)
        {
            Debug.Log("VICTORY!");
        }
    }
}
