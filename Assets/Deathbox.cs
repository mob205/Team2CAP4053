using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathbox : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerHealth player))
        {
            player.Kill();
        }
    }
}
