using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadyUpManager : MonoBehaviour
{
    [SerializeField] private InputActionProperty _readyAction;

    private int _numPlayers;
    private int _numReady;

    private Dictionary<PlayerInput, bool> _readyStates = new();

    public void OnPlayerJoined(PlayerInput player)
    {
        player.actions[_readyAction.reference.action.id.ToString()].started += (context) => OnPlayerReadyUp(player, context);
        ++_numPlayers;
    }

    private void OnPlayerReadyUp(PlayerInput player, InputAction.CallbackContext context)
    {
        if(!_readyStates.ContainsKey(player))
        {
            _readyStates.Add(player, false);
        }
        _readyStates[player] = !_readyStates[player];

    }
}