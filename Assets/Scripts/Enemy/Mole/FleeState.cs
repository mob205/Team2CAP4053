using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class FleeState : IState
{
    private NavMeshAgent _navAgent;
    private PlayerDetector _playerDetector;
    private float _speed;
    private LayerMask _viewBlocking;
    private ToolType _fleeToolType;
    private MoleAI _mole;
    private Animator _animator;

    private Func<PlayerInteractor, bool> _isValidTarget;


    public FleeState(MoleAI mole, NavMeshAgent agent, PlayerDetector playerDetector, float moveSpeed, LayerMask viewBlocking, ToolType fleeToolType, Animator animator)
    {
        _mole = mole;
        _navAgent = agent;
        _playerDetector = playerDetector;
        _speed = moveSpeed;
        _viewBlocking = viewBlocking;
        _fleeToolType = fleeToolType;
        _animator = animator;

    }
    public void Enter()
    {
        _navAgent.speed = _speed;

        _animator.SetBool("isWalking", true);
        _animator.SetBool("isAttacking", false);

        _isValidTarget = (player) =>
        {
            var diff = _navAgent.transform.position - player.transform.position;
            return (!Physics.Raycast(_navAgent.transform.position, diff.normalized, diff.magnitude, _viewBlocking) // Line of sight check
                && player.GetHeldToolType() == _fleeToolType); 
        };


    }

    public void Exit()
    {

    }

    public void Tick()
    {
        _mole.Target = Helpers.GetClosest(_navAgent.transform, _playerDetector.NearbyPlayers, _isValidTarget);

        if(!_mole.Target) { return; }
        Vector3 dirToPlayer = _navAgent.transform.position - _mole.Target.transform.position;
        Vector3 newPos = _navAgent.transform.position + dirToPlayer;

        _navAgent.SetDestination(newPos);
    }
}
