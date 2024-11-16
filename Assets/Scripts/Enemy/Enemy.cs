using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnKilled;
    public UnityEvent<Enemy> UnityOnKilled;
    public virtual void Kill()
    {
        OnKilled?.Invoke(this);
        UnityOnKilled?.Invoke(this);
        Destroy(gameObject);
    }
}
