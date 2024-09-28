using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : Activator
{
    [Tooltip("Euler angles representing how much to rotate when active, in degrees")]
    [SerializeField] private Vector3 _rotationOffset;

    [Tooltip("The amount of time, in seconds, that it should take for the rotation to happen")]
    [SerializeField] private float _transitionTime;

    [SerializeField] private Transform _rotationTarget;

    private Quaternion _initRot;
    private float _timer;

    private void Start()
    {
        _initRot = _rotationTarget.rotation;
    }

    private void Update()
    {
        if(_isActive)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _timer -= Time.deltaTime;
        }
        _timer = Mathf.Clamp(_timer, 0, _transitionTime);

        Debug.Log(_timer);

        Quaternion targetRot = _initRot * Quaternion.Euler(_rotationOffset);
        _rotationTarget.rotation = Quaternion.Slerp(_initRot, targetRot, _timer / _transitionTime);
    }

    protected override void Activate()
    {

    }

    protected override void Deactivate()
    {

    }
}
