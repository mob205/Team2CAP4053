using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class CatAI : Tool
{
    [Tooltip("The area the cat will walk toward when not held")]
    [SerializeField] private Transform _catTarget;

    [Tooltip("The maximum amount of time a player can hold the cat before the cat breaks free")]
    [SerializeField] private float _catMaxHoldDuration;

    [Tooltip("The minimum amount of time a player can hold the cat before the cat breaks free")]
    [SerializeField] private float _catMinHoldDuration;

    [SerializeField] private Collider _teleportCollider;

    private NavMeshAgent _agent;

    private float _holdTimeRemaining;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    protected override void Update()
    {
        base.Update();
        _agent.SetDestination(_catTarget.position);

        if(_isHeld && _holdTimeRemaining >= 0)
        {
            _holdTimeRemaining -= Time.deltaTime;
        }
        else if(_isHeld && _holdTimeRemaining < 0)
        {
            // Forces player to drop tool
            _heldPlayer.UnequipTool();
        }
    }
    public override void StartInteract(PlayerInteractor player)
    {
        base.StartInteract(player);

        _agent.isStopped = true;
        _teleportCollider.enabled = false;

        _holdTimeRemaining = Random.Range(_catMinHoldDuration, _catMaxHoldDuration);

    }

    public override void OnDropTool()
    {
        base.OnDropTool();

        _agent.isStopped = false;
        _agent.Warp(_agent.transform.position);
        _teleportCollider.enabled = true;
    }
}
