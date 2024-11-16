using UnityEngine.Events;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Layers to ignore collisions with when dead")]

    [SerializeField] private BoxCollider _mainCollider;
    [SerializeField] private BoxCollider _interactableCollider;
    [SerializeField] private BoxCollider _reviveCollider;

    [SerializeField] private LayerMask _deathIgnoreLayers;

    [SerializeField] private bool DebugIsInvulnerable = false;

    public bool IsDead { get; private set; }

    public UnityEvent<PlayerHealth> OnDeath;
    public UnityEvent<PlayerHealth> OnRevive;

    private void Start()
    {
        _reviveCollider.enabled = false;
    }
    public void Kill()
    {
        if (!IsDead && !DebugIsInvulnerable)
        {
            IsDead = true;
            OnDeath?.Invoke(this);

            _mainCollider.excludeLayers |= _deathIgnoreLayers;
            _interactableCollider.enabled = false;
            _reviveCollider.enabled = true;
        }
        
    }

    public void Revive()
    {
        IsDead = false;
        OnRevive?.Invoke(this);

        _mainCollider.excludeLayers &= ~_deathIgnoreLayers;
        _interactableCollider.enabled = true;
        _reviveCollider.enabled = false;
    }
}
