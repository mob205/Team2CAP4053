using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ReadyUpManager : MonoBehaviour
{
    [SerializeField] private InputActionProperty _readyAction;

    private int _numPlayers;
    private int _numReady;

    private Dictionary<PlayerInput, bool> _readyStates = new();

    public static event Action OnReadyUp;

    private bool _hasStarted;

    public void OnPlayerJoined(PlayerInput player)
    {
        player.actions[_readyAction.reference.action.id.ToString()].started += (context) => OnPlayerReadyUp(player, context);
        _readyStates.Add(player, false);
        ++_numPlayers;
    }

    private void OnPlayerReadyUp(PlayerInput player, InputAction.CallbackContext context)
    {
        if(!_hasStarted)
        {
            _readyStates[player] = !_readyStates[player];
            TryReady();
        }
    }

    private void TryReady()
    {
        foreach(var readyStatus in _readyStates.Values)
        {
            if(!readyStatus) { return; }
        }
        _hasStarted = true;
        OnReadyUp?.Invoke();
    }
}