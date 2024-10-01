using UnityEngine.Events;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;

    public bool IsDead { get; private set; }

    public UnityEvent<PlayerHealth> OnDeath;
    public UnityEvent<PlayerHealth> OnRevive;


    private void OnCollisionEnter(Collision collision)
    {
        if(!IsDead && Helpers.IsInMask(_enemyLayer, collision.gameObject.layer))
        {
            IsDead = true;
            OnDeath?.Invoke(this);
        }
    }

    public void Revive()
    {
        IsDead = false;
        OnRevive?.Invoke(this);
    }
}
