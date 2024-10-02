using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoleAI : MonoBehaviour
{
    public enum MoleState
    {
        Patrolling,
        Fleeing,
        Attacking,
        Chasing,
    }

    public MoleState CurrentState { get; private set; }

    [Tooltip("Type of tool the mole flees from")]
    [SerializeField] private ToolType _fleeTool;


    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _walkPointRange;
    [SerializeField] private float _sightRange;

    [Tooltip("Speed to move at when chasing or fleeing")]
    [SerializeField] private float _runSpeed;

    [Tooltip("Speed to move at when patrolling")]
    [SerializeField] private float _walkSpeed;

    //[SerializeField] private float _attackRange;

    // Patrolling
    private Vector3 _walkPoint;
    private bool _isWalkPointSet;


    //Attacking
    [SerializeField] private float _timeBetweenAttacks;

    private bool _hasAttacked;
    private NavMeshAgent _navAgent;

    public void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        // Check if player is inside attack range
        var hits = Physics.SphereCastAll(transform.position, _sightRange, Vector3.forward, Mathf.Epsilon, _playerLayer);

        if (CheckFlee(hits, out Transform fleePos)) // Nearby tool to flee from
        {
            Flee(fleePos);
            return;
        }

        Transform closestPlayer = FindClosestPlayer(hits);
        if(closestPlayer == null)
        {
            Patrol();
        }
        else
        {
            ChasePlayer(closestPlayer);
        }
    }

    private bool CheckFlee(RaycastHit[] nearbyPlayers, out Transform pos)
    {
        foreach(var player in nearbyPlayers)
        {
            if(player.collider.gameObject.TryGetComponent(out PlayerInteractor interactor) && interactor.GetHeldToolType() == _fleeTool)
            {
                pos = interactor.transform;
                return true;
            }
        }
        pos = null;
        return false;
    }

    // Move away from fleePosition
    private void Flee(Transform fleePosition)
    {
        CurrentState = MoleState.Fleeing;
        _navAgent.speed = _runSpeed;

        // Vector player to me
        Vector3 dirToPlayer = transform.position - fleePosition.position;
        Vector3 newPos = transform.position + dirToPlayer;

        _navAgent.SetDestination (newPos);
    }

    private void Patrol()
    {
        CurrentState = MoleState.Patrolling;
        _navAgent.speed = _walkSpeed;

        if (!_isWalkPointSet) SearchWalkPoint();
            _navAgent.SetDestination(_walkPoint);

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            _isWalkPointSet = false;
    }

    private void ChasePlayer(Transform target)
    {
        CurrentState = MoleState.Chasing;
        _navAgent.speed = _runSpeed;

        _navAgent.SetDestination(target.position);
    }

    private void SearchWalkPoint()
    {
        // calculate random point in range
        float randomZ = UnityEngine.Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = UnityEngine.Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _groundLayer))
            _isWalkPointSet = true;
    }

    private bool IsValidPlayer(RaycastHit player)
    {
        // Perform line of sight raycast
        if(player.collider.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            return !playerHealth.IsDead;
        }
        return false;
    }

    private Transform FindClosestPlayer(RaycastHit[] players)
    {
        Transform best = null;
        float minDist = Mathf.Infinity;

        foreach (var player in players)
        {
            if(!IsValidPlayer(player)) { continue; }
            float dist = (player.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                best = player.transform;
                minDist = dist;
            }
        }
        return best;
    }

    //private void attackPlayer()
    //{
    //    moleState = "attacking";

    //    // make sure enemy doesn't move
    //    agent.SetDestination(transform.position);

    //    transform.LookAt(closestPlayer);

    //    if (!alreadyAttacked)
    //    {
    //        // ATTACK CODE HERE

    //        // ^^^^^

    //        alreadyAttacked = true;
    //        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //    }
    //}

    //private void ResetAttack()
    //{
    //    _hasAttacked = false;
    //}
}
