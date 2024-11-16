using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OracleAI : Enemy
{
    [SerializeField] private float _breakChanceIncrease;

    private EnemySpawner[] _spawners;
    private void Start()
    {
        _spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
        ApplyEffect(_breakChanceIncrease);
        OnKilled += (Enemy) => ApplyEffect(-_breakChanceIncrease);
    }

    private void ApplyEffect(float modifier)
    {
        foreach(var spawner in _spawners)
        {
            spawner.AddBreakChanceModifier(modifier);
        }
    }
}
