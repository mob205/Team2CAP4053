using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHover : MonoBehaviour
{
    [SerializeField] private float _amplitude;
    [SerializeField] private float _period;
    private Vector3 _initialPos;

    private void Start()
    {
        _initialPos = transform.position;
        ApplyHover();
    }
    private void FixedUpdate()
    {
        ApplyHover();
    }
    private void ApplyHover()
    {
        if (_period == 0) { return; }
        var hoverDist = (_amplitude * Mathf.Sin(((2 * Mathf.PI) / _period) * Time.time));
        transform.position = _initialPos + transform.rotation * new Vector3(0, hoverDist, 0);
    }
}
