using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Start()
    {
        if(_target == null)
        {
            Debug.LogError($"No follow target found on {name}");
            Destroy(gameObject);
        }
        transform.SetParent(null, true);
    }
    private void Update()
    {
        transform.position = _target.position;
    }
}
