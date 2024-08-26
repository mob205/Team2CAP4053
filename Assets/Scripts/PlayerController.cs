using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{
    private Movement _movement;

    // TODO: there's probably a better way to generate unique IDs, if they're even needed at all
    private static int nextID = 0;
    private int _playerID;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _playerID = nextID++;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _movement.InputMove(context.ReadValue<Vector2>());
    }
    public void OnUse(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log($"{_playerID} pressed A!");
        }
    }
}
