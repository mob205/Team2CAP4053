using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ReadyUpManager : MonoBehaviour
{
    [SerializeField] private InputActionProperty _readyAction;

    private int _numPlayers;
    private int _numReady;

    public List<KeyValuePair<PlayerInput, bool>> ReadyStates { get; private set; } = new();

    public UnityEvent OnReadyUp;
    public UnityEvent OnReadyStateChange;

    private bool _hasStarted;

    public static ReadyUpManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        player.actions[_readyAction.reference.action.id.ToString()].started += (context) => OnPlayerReadyUp(player, context);
        ReadyStates.Add(new KeyValuePair<PlayerInput, bool>(player, false));
        ++_numPlayers;
        OnReadyStateChange?.Invoke();
    }

    private void OnPlayerReadyUp(PlayerInput player, InputAction.CallbackContext context)
    {
        if(!_hasStarted)
        {
            var idx = ReadyStates.FindIndex(item => item.Key == player);
            ReadyStates[idx] = new KeyValuePair<PlayerInput, bool>(player, !ReadyStates[idx].Value);
            OnReadyStateChange?.Invoke();
            TryReady();
        }
    }

    private void TryReady()
    {
        foreach (var readyStatus in ReadyStates)
        {
            if(!readyStatus.Value) { return; }
        }
        _hasStarted = true;
        OnReadyUp?.Invoke();
    }
}