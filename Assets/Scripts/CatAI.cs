using System.Collections;
using System.Collections.Generic;
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

    private NavMeshAgent _agent;

    private float _holdTimeRemaining;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        _agent.SetDestination(_catTarget.position);
    }
    protected override void Update()
    {
        base.Update();
        if(_isHeld && _holdTimeRemaining >= 0)
        {
            Debug.Log("B");
            _holdTimeRemaining -= Time.deltaTime;
        }
        else if(_isHeld && _holdTimeRemaining < 0)
        {
            Debug.Log("A");
            // Forces player to drop tool
            _heldPlayer.UnequipTool();
        }
    }
    public override void StartInteract(PlayerInteractor player)
    {
        base.StartInteract(player);

        _agent.isStopped = true;
        _holdTimeRemaining = Random.Range(_catMinHoldDuration, _catMinHoldDuration);
    }

    public override void OnDropTool()
    {
        Debug.Log("EFGH");
        base.OnDropTool();

        _agent.isStopped = false;
    }
}
