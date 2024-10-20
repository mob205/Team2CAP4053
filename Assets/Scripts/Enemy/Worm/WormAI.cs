using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WormAI : MonoBehaviour
{
    [Tooltip("Max amount the worm can rotate, in degrees per second")]
    [SerializeField] private float _turnSpeed;

    [Tooltip("Amount the worm travels per second")]
    [SerializeField] private float _speed;

    [Tooltip("Delay to wait before rechecking all possible targets")]
    [SerializeField] private float _retargetDelay;

    [Tooltip("Amount of time before the worm will stop attacking and return to the return point")]
    [SerializeField] private float _attackLifetime;

    [Tooltip("Delay after worm finishes attacking when the worm despawns")]
    [SerializeField] private float _despawnDelay;

    [Tooltip("Position the worm will go to before despawning")]
    [SerializeField] private Vector3 _wormReturnPoint;

    private Transform _target;
    private float _attackTimer;

    private void Start()
    {
        StartCoroutine(DelayedRetarget());
        _attackTimer = _attackLifetime;
    }

    private void Update()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0 && _target)
        {
            MoveToTarget(_target.position);
        }
        else
        {
            MoveToTarget(_wormReturnPoint);

        }

        if(_attackTimer < -_despawnDelay)
        {
            Destroy(gameObject);
        }
    }

    private void MoveToTarget(Vector3 target)
    {
        var diff = target - transform.position;
        var targetRot = Quaternion.LookRotation(diff);

        transform.SetPositionAndRotation(
            transform.position + (_speed * Time.deltaTime * transform.forward),
            Quaternion.RotateTowards(transform.rotation, targetRot, _turnSpeed * Time.deltaTime));
    }

    // Finds the player that is most "in front" of the worm (i.e. requires the least rotation to face)
    // Player must be alive to be eligible for targetting
    private Transform FindClosestPlayer()
    {
        var players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);

        Transform best = null;
        float bestW = 0;

        foreach(var player in players)
        {
            if (player.IsDead) continue;
            var diff = player.transform.position - transform.position;

            // w components further from 0 signify less rotation
            var curW = Mathf.Abs(Quaternion.FromToRotation(transform.forward, diff).w);
            if(curW > bestW)
            {
                best = player.transform;
                bestW = curW;
            }
        }
        return best;
    }

    private IEnumerator DelayedRetarget()
    {
        while(true)
        {
            _target = FindClosestPlayer();
            yield return new WaitForSeconds(_retargetDelay);
        }
    }
}
