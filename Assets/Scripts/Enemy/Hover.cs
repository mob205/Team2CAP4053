using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{

    [Tooltip("Rate at which the enemy rotates, in degrees per second")]
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _hoverAmplitude;
    [SerializeField] private float _hoverPeriod;

    private Vector3 _initialPos;
    private void Start()
    {
        Debug.Log("Oracle spawned!");
        _initialPos = transform.position;
    }
    private void FixedUpdate()
    {
        transform.Rotate(transform.up, _rotationSpeed * Time.fixedDeltaTime);

        if (_hoverPeriod == 0) { return; }
        var hoverY = _initialPos.y + (_hoverAmplitude * Mathf.Sin(((2 * Mathf.PI) / _hoverPeriod) * Time.time));
        transform.position = new Vector3(_initialPos.x, hoverY, _initialPos.z);
    }
}
