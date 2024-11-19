using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathState : IState
{
    private NavMeshAgent _agent;
    public DeathState(NavMeshAgent agent)
    {
        _agent = agent;
    }
    public void Enter()
    {
        _agent.isStopped = true;
    }

    public void Exit()
    {
    }

    public void Tick()
    {
    }
}
