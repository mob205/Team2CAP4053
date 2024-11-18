using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathbox : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private ToolType _ignoreToolType;

    private void OnTriggerEnter(Collider other)
    {
        if(_ignoreToolType != null && other.TryGetComponent(out PlayerInteractor interactor) && interactor.GetHeldToolType() == _ignoreToolType)
        {
            return;
        }
        if(other.TryGetComponent(out PlayerHealth player))
        {
            player.Kill();
        }
    }
}
