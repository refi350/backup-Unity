using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private StateMachine brain;
    private Animator animator;
    [SerializeField]
    private Text stateNote;
    private NavMeshAgent agent;
    private Player player;
    private bool playerIsNear;
    private bool withinAttackRange;
    private float changeMind;
    private float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        brain = GetComponent<StateMachine>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
        playerIsNear = false;
        withinAttackRange = false;
        brain.PushState(Idle, OnIdleEnter, OnIdleExit);
    }

    // Update is called once per frame
    void Update()
    {
        playerIsNear = Vector3.Distance(transform.position, player.transform.position) < 5;
        withinAttackRange = Vector3.Distance(transform.position, player.transform.position) < 1;
    }

    void OnIdleEnter()
    {
        stateNote.text = "Idle";
        agent.ResetPath();        
    }

    void Idle()
    {       
        changeMind -= Time.deltaTime;
        if (playerIsNear)
        {
            brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
        if(changeMind <= 0)
        {
            brain.PushState(Wander, OnWanderEnter, OnWanderExit);
            changeMind = Random.Range(4f, 10f);
        }
    }

    void OnIdleExit()
    {

    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
        if(Vector3.Distance(transform.position,player.transform.position) > 5.5f)
        {
            brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }
        if (withinAttackRange)
        {
            brain.PushState(Attack, OnAttackEnter, null);
        }
    }

    void OnChaseEnter()
    {
        animator.SetBool("Chase", true);
        stateNote.text = "Chase";
    }

    void OnChaseExit()
    {
        animator.SetBool("Chase", false);
    }

    void Wander()
    {
        
        if(agent.remainingDistance <= .25f)
        {
            agent.ResetPath();
            brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }
        if (playerIsNear)
        {
            brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
    }

    void OnWanderEnter()
    {
        stateNote.text = "Wander";
        animator.SetBool("Chase", true);
        Vector3 wanderDirection = (Random.insideUnitSphere * 4f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(wanderDirection, out navMeshHit,3f,NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        agent.SetDestination(destination);
    }

    void OnWanderExit()
    {
        animator.SetBool("Chase", false);
    }

    void Attack()
    {
        attackTimer -= Time.deltaTime;
        if (!withinAttackRange)
        {
            brain.PopState();
        }else if(attackTimer <= 0)
        {
            animator.SetTrigger("Attack");
            player.Hurt(4,1);
            attackTimer = 2f;
        }
    }
    void OnAttackEnter()
    {
        agent.ResetPath();
        stateNote.text = "Attack";
    }

}
