using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ChaseState : IState
{
    private float _speed;
    private NavMeshAgent _navAgent;
    private PlayerDetector _playerDetector;
    private LayerMask _viewBlocking;
    private MoleAI _mole;
    private Animator _animator;


    public ChaseState(MoleAI mole, NavMeshAgent agent, PlayerDetector playerDetector, float moveSpeed, LayerMask viewBlocking, Animator animator)
    {
        _mole = mole;
        _speed = moveSpeed;
        _navAgent = agent;
        _playerDetector = playerDetector;
        _viewBlocking = viewBlocking;
        _animator = animator;
    }

    public void Enter()
    {
        _navAgent.speed = _speed;

        _animator.SetBool("isWalking", true);
        _animator.SetBool("isAttacking", false);

        Func<PlayerInteractor, bool> _isValidTarget = (player) =>
        {
            var diff = player.transform.position - _navAgent.transform.position;
            return !Physics.Raycast(_navAgent.transform.position, diff.normalized, diff.magnitude, _viewBlocking); // Line of sight check
        };

        _mole.Target = Helpers.GetClosest(_navAgent.transform, _playerDetector.NearbyPlayers, _isValidTarget);
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        if(_mole.Target)
        {
            _navAgent.SetDestination(_mole.Target.transform.position);
        }
    }
}
