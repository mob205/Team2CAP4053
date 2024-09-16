using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoleAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public void Awake()
    {
        // Player object goes here
        player = GameObject.Find("Player").transform;
        Debug.Log("Player = " + player);

        agent = GetComponent<NavMeshAgent>();
        Debug.Log("Awake!");
    }

    public void Update()
    {
        // Check if player is inside attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            // Debug.Log("Patroling!");
            patroling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            // Debug.Log("Enemy Spotted!");
            chasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            // Debug.Log("Attacking Player");
            attackPlayer();
        }
    }

    private void patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // calculate random point in range
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void chasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void attackPlayer()
    {
        // make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // ATTACK CODE HERE

            // ^^^^^

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


}
