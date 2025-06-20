using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] int damage;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;

    private Transform player;
    private float lastAttackTime;
    private Rigidbody2D rb;

    private Vector2 movement;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

void Update()
{
    /*
    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    if (distanceToPlayer > attackRange)
    {
        // Hedef yönü hesapla (her karede)
        movement = (player.position - transform.position).normalized;
    }
    else
    {
        movement = Vector2.zero;
    }

    // Oyuncuya X ekseninde dön
    Vector3 scale = transform.localScale;
    scale.x = player.position.x > transform.position.x ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
    transform.localScale = scale;*/
}


    void FixedUpdate()
    {
        agent.SetDestination(player.position);
        /*
        // Hareketi sabit fizik update'inde uygula
        if (movement != Vector2.zero)
        {
            Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }*/
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
        yield return new WaitForSeconds(0.3f);
    }
}
