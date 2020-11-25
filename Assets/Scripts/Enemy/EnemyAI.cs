using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float enemyDamage = 10f;
    [SerializeField] float attackDistance = 3f;
    [SerializeField] float chaseRange = 5f;

    Transform target;
    NavMeshAgent navMeshAgent;
    Animator anim;
    PlayerHealth playerHealth;
    BossSpawner bossSpawner;

    float distanceToTarget = Mathf.Infinity;
    public bool isProvoked = false;

    void Start()
    {
        bossSpawner = FindObjectOfType<BossSpawner>(); 
        bossSpawner.killPoints += 1;
        target = FindObjectOfType<CharacterControl>().transform;
        anim = GetComponent<Animator>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(playerHealth.transform.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if(distanceToTarget <= chaseRange)
        {
            isProvoked = true;
            navMeshAgent.SetDestination(target.position);
        }

        

        navMeshAgent.stoppingDistance = attackDistance;


        AttackAnimation();
        RunAnimation();
    }

    private void EngageTarget()
    {
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }        
    }

    private void ChaseTarget()
    {
        navMeshAgent.SetDestination(playerHealth.transform.position);
    }

    void RunAnimation()
    {
        if (navMeshAgent.velocity.magnitude > 0f)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    void AttackAnimation()
    {
        if(Vector3.Distance(transform.position, target.transform.position) < attackDistance)
        {
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }

    public void EnemyAttackHitEvent()
    {
        playerHealth.PlayerTakeDamage(enemyDamage);
    }
}
