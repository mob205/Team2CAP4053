using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class MoleAI : MonoBehaviour
{
   
    public PlayerInteractor Target { get; set; }

    [Tooltip("Type of tool the mole flees from")]
    [SerializeField] private ToolType _fleeTool;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _viewBlocking;

    [SerializeField] private float _walkPointRange;

    [Tooltip("Speed to move at when chasing or fleeing")]
    [SerializeField] private float _runSpeed;

    [Tooltip("Speed to move at when patrolling")]
    [SerializeField] private float _walkSpeed;

    private StateMachine _stateMachine;


    //Attacking
    [SerializeField] private float _timeBetweenAttacks;

    private PlayerDetector _playerDetector;
    private NavMeshAgent _navAgent;
    private Animator _animator;

    public void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _playerDetector = GetComponent<PlayerDetector>();
        _animator = GetComponent<Animator>();

        _stateMachine = new StateMachine();

        // Create states
        var chase = new ChaseState(this, _navAgent, _playerDetector, _runSpeed, _viewBlocking, _animator);
        var patrol = new PatrolState(_navAgent, _walkSpeed, _walkPointRange, _groundLayer, _animator);
        var flee = new FleeState(this, _navAgent, _playerDetector, _runSpeed, _viewBlocking, _fleeTool, _animator);

        _stateMachine.AddTransition(patrol, flee, HasValidFleeTarget);
        _stateMachine.AddTransition(patrol, chase,  () => { return !HasValidFleeTarget() && HasAnyPlayerInSight(); });

        _stateMachine.AddTransition(flee, patrol,   () => { return HasNoTarget() || !IsPlayerAlive(Target); });
        _stateMachine.AddTransition(flee, chase,    () => { return !PlayerHasFleeTool(Target); });

        _stateMachine.AddTransition(chase, flee,    () => { return PlayerHasFleeTool(Target) && IsPlayerInSight(Target); });
        _stateMachine.AddTransition(chase, patrol,  () => { return HasNoTarget() || !IsPlayerAlive(Target); });

        _stateMachine.SetState(patrol);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public bool IsPlayerInSight(PlayerInteractor player)
    {
        var diff = player.transform.position - transform.position;
        return !Physics.Raycast(transform.position, diff.normalized, diff.magnitude, _viewBlocking);
    }

    public bool PlayerHasFleeTool(PlayerInteractor player)
    {
        if (!player) { return false; }
        return player.GetHeldToolType() == _fleeTool;
    }

    public bool HasValidFleeTarget()
    {
        return Helpers.HasAny(_playerDetector.NearbyPlayers, (player) => { return IsPlayerInSight(player) && PlayerHasFleeTool(player) && IsPlayerAlive(player); });
    }
    public bool HasAnyPlayerInSight()
    {
        return Helpers.HasAny(_playerDetector.NearbyPlayers, (player) => { return IsPlayerInSight(player) && IsPlayerAlive(player); });
    }

    private bool IsPlayerAlive(PlayerInteractor player)
    {
        return (player.TryGetComponent(out PlayerHealth health) && !health.IsDead);
    }
    private bool HasNoTarget()
    {
        return Target == null;
    }
}
