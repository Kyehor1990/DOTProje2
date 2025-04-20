using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] int damage;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;
    
    private Transform player;
    private bool isAttacking = false;
    private float lastAttackTime;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer > attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AttackPlayer(other);
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AttackPlayer(other);
        }
    }
    
    void AttackPlayer(Collider2D playerCollider)
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            playerCollider.GetComponent<PlayerHealth>().TakeDamage(damage);
            lastAttackTime = Time.time;
            
            StartCoroutine(AttackEffect());
        }
    }
    
    IEnumerator AttackEffect()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }
}