using UnityEngine.Events;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private BoxCollider _mainCollider;

    [SerializeField] private bool DebugIsInvulnerable = false;

    public bool IsDead { get; private set; }

    public UnityEvent<PlayerHealth> OnDeath;
    public UnityEvent<PlayerHealth> OnRevive;

    public void Kill()
    {
        if (!IsDead && !DebugIsInvulnerable)
        {
            IsDead = true;
            OnDeath?.Invoke(this);
            _mainCollider.excludeLayers |= _playerLayer;
        }
        
    }

    public void Revive()
    {
        IsDead = false;
        OnRevive?.Invoke(this);

        _mainCollider.excludeLayers &= ~_playerLayer;
    }
}
