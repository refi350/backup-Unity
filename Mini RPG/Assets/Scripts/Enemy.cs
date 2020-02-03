using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int curHP;
    public int maxHP;
    public float moveSpeed;
    public int xpToGive;

    [Header("Target")]
    public float chaseRange;
    public float attackRange;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    private Rigidbody2D rig;
    private Player player;


    private void Awake()
    {        
        rig = GetComponent<Rigidbody2D>();
    }

    private void Start() 
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        float playerDist = Vector2.Distance(transform.position, player.transform.position);

        if(playerDist <= attackRange)
        {
            if(Time.time - lastAttackTime >= attackRate)
            {
                Attack();
            }

            rig.velocity = Vector2.zero;
        }
        else if(playerDist <= chaseRange)
        {
            Chase();
        }
        else
        {
            rig.velocity = Vector2.zero;
        }
    }

    void Chase()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;

        rig.velocity = dir * moveSpeed;
    }

    public void TakeDamage(int damageTaken)
    {
        curHP -= damageTaken;

        if(curHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        player.AddXP(xpToGive);

        Destroy(gameObject);
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        player.TakeDamage(damage);
    }
}
