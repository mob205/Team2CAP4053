using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnKilled;
    public virtual void Kill()
    {
        OnKilled?.Invoke(this);
        Destroy(gameObject);
    }
}
