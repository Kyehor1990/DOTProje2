using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int damage = 1;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    
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
        //saldırı animasyonu
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }
}