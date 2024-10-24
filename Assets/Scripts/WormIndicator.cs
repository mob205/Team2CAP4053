using System;
using UnityEngine.Events;
using UnityEngine;

public class WormIndicator : MonoBehaviour
{
    public UnityEvent<WormAI> OnWormEnter;
    public UnityEvent<WormAI> OnWormExit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out WormAI worm))
        {
            OnWormEnter?.Invoke(worm);
            worm.OnKilled += OnWormKilled;
            worm.OnWormAttack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out WormAI worm))
        {
            OnWormExit?.Invoke(worm);
        }
    }

    private void OnWormKilled(Enemy enemy)
    {
        if(enemy.TryGetComponent(out WormAI worm))
        {
            OnWormExit?.Invoke(worm);
        }
    }

}
