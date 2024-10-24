using UnityEngine.Events;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Layers to ignore collisions with when dead")]
    [SerializeField] private LayerMask _deathIgnoreLayers;

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
            _mainCollider.excludeLayers |= _deathIgnoreLayers;
        }
        
    }

    public void Revive()
    {
        IsDead = false;
        OnRevive?.Invoke(this);

        _mainCollider.excludeLayers &= ~_deathIgnoreLayers;
    }
}
