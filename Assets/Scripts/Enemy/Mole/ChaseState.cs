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

    private Vector3 _lastValidTarget;


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
        Debug.Log(_navAgent.nextPosition);
        if(_mole.Target)
        {
            var path = new NavMeshPath();
            _navAgent.CalculatePath(_mole.Target.transform.position, path);
            if(path.status == NavMeshPathStatus.PathComplete)
            {
                _navAgent.SetPath(path);
                _lastValidTarget = _mole.Target.transform.position;
                Debug.Log("Success!");
            }
            else
            {
                Debug.Log("Failure");
                _navAgent.SetDestination(_lastValidTarget);
            }
            //_navAgent.CalculatePath
            //_navAgent.SetDestination(_mole.Target.transform.position);

            //if(_navAgent.path.status == NavMeshPathStatus.PathComplete)
            //{
            //    Debug.Log("Path is complete.");
            //    _lastValidLocation = _mole.Target.transform.position;
            //}
            //else
            //{
            //    Debug.Log("Path is incomplete.");
            //    _navAgent.SetDestination(_lastValidLocation);
            //}
            //if(_navAgent.path.status != NavMeshPathStatus.PathComplete)
            //{
            //    _navAgent.SetDestination(_lastValidLocation);
            //}
            //else
            //{
            //    _lastValidLocation = _mole.Target.transform.position;
            //}
        }
    }
}
