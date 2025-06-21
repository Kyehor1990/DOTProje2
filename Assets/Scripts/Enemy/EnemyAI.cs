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
    NavMeshAgent agent;

    [SerializeField] private AudioClip[] randomSounds;
    private AudioSource audioSource;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0.3f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(PlayRandomSoundLoop());
    }


    void FixedUpdate()
    {
        agent.SetDestination(player.position);

        if (player == null) return;

        Vector3 direction = player.position - transform.position;
    
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Sign(direction.x) * Mathf.Abs(scale.x); // Sağdaysa pozitif, soldaysa negatif
        transform.localScale = scale;
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

    IEnumerator PlayRandomSoundLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(5f, 10f); // 5-10 saniye bekle
            yield return new WaitForSeconds(waitTime);

            if (randomSounds.Length > 0 && audioSource != null)
            {
                int index = Random.Range(0, randomSounds.Length);
                AudioClip selectedClip = randomSounds[index];
                if (selectedClip != null)
                {
                    audioSource.PlayOneShot(selectedClip);
                }
            }
        }
    }


}
