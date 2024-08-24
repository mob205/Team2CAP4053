using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    // Properties
    [field: SerializeField] public float MaxSpeed { get; private set; } = 10f;
    [field: SerializeField] public float Acceleration { get; private set; } = 150f;
    [field: SerializeField] public float Deceleration { get; private set; } = 100f;


    // References
    private PlayerInput _playerInput;
    private Rigidbody _rb;

    private static int nextID = 0;

    // Private values
    private int _playerID;
    private Vector2 _moveInput;
    private Vector2 _frameVelocity;

    private void Awake()
    { 
        _playerID = nextID++;
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void OnUse(InputAction.CallbackContext context)
    {
        Debug.Log($"{_playerID} pressed A!");
    }
    private void FixedUpdate()
    {
        ProcessMovement();
    }
    public void ProcessMovement()
    {
        if (_moveInput == Vector2.zero)
        {
            _frameVelocity = Vector2.MoveTowards(_frameVelocity, Vector2.zero, Deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity = Vector2.MoveTowards(_frameVelocity, _moveInput * MaxSpeed, Acceleration * Time.fixedDeltaTime);
        }
        _rb.velocity = new Vector3(_frameVelocity.x, 0, _frameVelocity.y);
    }
}
