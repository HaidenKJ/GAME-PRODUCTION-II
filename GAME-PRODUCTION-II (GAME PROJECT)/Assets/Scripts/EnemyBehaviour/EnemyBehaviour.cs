using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent agent; // Finna try using NavMesh
    public Transform player;
    public LayerMask WhatIsGround, WhatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointR;

    // Attacking 
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerBody").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        //Checking for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) Patrol();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInSightRange && playerInAttackRange) AttackPlayer();

    }
    private void Patrol()
    {
        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint Reached
        if(distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        // Make sure enemy Doesnt Move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            //Attack Code here


            //....
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = new Vector3(5.71f,0f,0f);
        Debug.Log("RRRAAAAAAAAAAAH");

    }

    private void SearchWalkPoint()
    {
        // Calculate random point
        float randomZ = Random.Range(-walkPointR, walkPointR);
        float randomX = Random.Range(-walkPointR, walkPointR);
        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas)) // Check if point is valid
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }
}
