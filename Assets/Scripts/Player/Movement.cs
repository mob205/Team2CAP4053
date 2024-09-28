using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [field: SerializeField] public float MaxSpeed { get; set; } = 10f;
    [field: SerializeField] public float Acceleration { get; set; } = 150f;
    [field: SerializeField] public float Deceleration { get; set; } = 100f;

    private Rigidbody _rb;
   
    private Vector2 _moveInput;
    private Vector2 _frameVelocity;

    private float _facingAngle;

    private void Awake()
    { 
        _rb = GetComponent<Rigidbody>();
    }
    public void InputMove(Vector2 moveInput)
    {
        _moveInput = moveInput.normalized;
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
            _facingAngle = Mathf.Rad2Deg * Mathf.Atan2(-_frameVelocity.y, _frameVelocity.x);
        }
        _rb.velocity = new Vector3(_frameVelocity.x, 0, _frameVelocity.y);
        transform.rotation = Quaternion.Euler(0, _facingAngle, 0);

    }
}
