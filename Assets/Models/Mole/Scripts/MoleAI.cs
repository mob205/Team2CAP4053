using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoleAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player1;
    public Transform player2;
    public Transform player3;
    public Transform player4;
    public Transform hammer;
    public Transform closestPlayer;    


    public LayerMask whatIsGround, whatIsPlayer, whatIsHammer;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float sightRange, attackRange, hammerRange;
    public bool playerInSightRange, playerInAttackRange, hammerInRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public NavMeshData navMeshData;

    private string moleState;

    public void Awake()
    {
        // Player object and Hammer goes here
        player1 = GameObject.Find("Player1").transform;
        player2 = GameObject.Find("Player2").transform;
        player3 = GameObject.Find("Player3").transform;
        player4 = GameObject.Find("Player4").transform;
        hammer = GameObject.Find("Hammer").transform;

        agent = GetComponent<NavMeshAgent>();

        Debug.Log("Awake!");
    }

    public void Update()
    {
        // Find closest player
        closestPlayer = findClosestPlayer();

        // Check if player is inside attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        hammerInRange = Physics.CheckSphere(transform.position, hammerRange, whatIsHammer);

        if (hammerInRange) {
            flee();
        }

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

    public string getState() {
        return moleState;
    }

    private Transform findClosestPlayer() {

        Transform[] players = {player1, player2, player3, player4};

        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Transform t in players)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    private void flee()
    {
        moleState = "fleeing";

        // Vector player to me
        Vector3 dirToPlayer = transform.position - hammer.position;
        Vector3 newPos = transform.position + dirToPlayer;
        agent.SetDestination (newPos);
    }

    private void patroling()
    {
        moleState = "patroling";

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
        moleState = "chasing";
        agent.SetDestination(closestPlayer.position);
    }

    private void attackPlayer()
    {
        moleState = "attacking";
        
        // make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(closestPlayer);

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
