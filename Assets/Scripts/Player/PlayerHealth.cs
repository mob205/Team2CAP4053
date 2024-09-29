using UnityEngine.Events;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;

    public UnityEvent<PlayerHealth> OnDeath;

    private bool _isDead;

    private void OnCollisionEnter(Collision collision)
    {
        if(!_isDead && Helpers.IsInMask(_enemyLayer, collision.gameObject.layer))
        {
            _isDead = true;
            OnDeath?.Invoke(this);
        }
    }
}
