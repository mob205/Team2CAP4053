using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Activator : MonoBehaviour
{
    [Tooltip("Layers to check for activations")]
    [SerializeField] private LayerMask _activationLayers;

    [SerializeField] private UnityEvent _onActivate;
    [SerializeField] private UnityEvent _onDeactivate;

    private int _playerCount = 0;

    protected bool _isActive;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(Helpers.IsInMask(_activationLayers, other.gameObject.layer))
        {
            ++_playerCount;

            if (!_isActive)
            {
                _isActive = true;
                Activate();
                _onActivate?.Invoke();
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if(Helpers.IsInMask(_activationLayers, other.gameObject.layer))
        {
            --_playerCount;

            if (_playerCount == 0 && _isActive)
            {
                _isActive = false;
                Deactivate();
                _onDeactivate?.Invoke();
            }
        }
    }

    protected abstract void Activate();
    protected abstract void Deactivate();
}
