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

    private Rigidbody2D rb;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

    }
    
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer > attackRange)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, player.position, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);

        }
    }
    
   void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        AttackPlayer(collision.gameObject);
    }
}

void OnCollisionStay2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        AttackPlayer(collision.gameObject);
    }
}
    
   void AttackPlayer(GameObject player)
{
    if (Time.time - lastAttackTime >= attackCooldown)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            lastAttackTime = Time.time;
            StartCoroutine(AttackEffect());
        }
    }
}
    
    IEnumerator AttackEffect()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }
}