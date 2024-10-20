using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentFollower : MonoBehaviour
{

    [SerializeField] private float _followDistance;
    private Transform _prev;
    private Transform _head;

    private void Awake()
    {
        _prev = transform.parent;
        _head = transform.root;
    }

    void Start()
    {
        transform.parent = null;
    }

    void Update()
    {
        if(!_head || !_prev)
        {
            Destroy(gameObject);
            return;
        }

        var diff = transform.position - _prev.position;
        if (diff.magnitude > _followDistance)
        {
            transform.position = _prev.position + (diff.normalized * _followDistance);
            transform.LookAt(_prev);
        }
    }
}
