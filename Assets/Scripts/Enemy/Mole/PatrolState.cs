using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    private NavMeshAgent _navAgent;
    private float _speed;
    private float _patrolRange;
    private Animator _animator;

    private Vector3 _newWalkPoint;
    private Vector3 _setWalkPoint;
    private LayerMask _groundLayer;
    bool _isPatrolSet;

    public PatrolState(NavMeshAgent navAgent, float speed, float patrolRange, LayerMask groundLayer, Animator animator)
    {
        _navAgent = navAgent;
        _speed = speed;
        _patrolRange = patrolRange;
        _groundLayer = groundLayer;
        _animator = animator;
    }

    public void Enter()
    {
        _navAgent.speed = _speed;
        _animator.SetBool("isWalking", true);
        _animator.SetBool("isAttacking", false);
    }

    public void Exit()
    {
        _isPatrolSet = false;
    }

    public void Tick()
    {
        if (!_isPatrolSet) SetRandomTarget();
        if(_newWalkPoint != _setWalkPoint)
        {
            _navAgent.SetDestination(_newWalkPoint);
            _setWalkPoint = _newWalkPoint;
        }

        Vector3 distanceToWalkPoint = _navAgent.transform.position - _setWalkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            _isPatrolSet = false;
    }

    private void SetRandomTarget()
    {
        // calculate random point in range
        float randomZ = Random.Range(-_patrolRange, _patrolRange);
        float randomX = Random.Range(-_patrolRange, _patrolRange);

        var transform = _navAgent.transform;

        _newWalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_newWalkPoint, -transform.up, 2f, _groundLayer))
            _isPatrolSet = true;
    }
}
