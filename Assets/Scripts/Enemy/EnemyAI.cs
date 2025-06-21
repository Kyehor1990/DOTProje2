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
        agent.stoppingDistance = 0.1f; // Oyuncuya daha yakın dur
        agent.autoBraking = true; // Otomatik frenleme
        
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
        if (player == null || agent == null || !agent.enabled || !agent.isOnNavMesh) return;

        // Sürekli oyuncuyu takip et, ama fazla sık güncelleme yapma
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Sadece oyuncu belirli bir mesafeden uzaktaysa hedefi güncelle
        if (distanceToPlayer > 1f || !agent.hasPath || agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(player.position);
        }

        // Sprite yönlendirme
        Vector3 direction = player.position - transform.position;
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void OnEnable()
    {
        // Script yeniden etkinleştirildiğinde NavMeshAgent'ı kontrol et
        StartCoroutine(CheckNavMeshAgent());
    }

    IEnumerator CheckNavMeshAgent()
    {
        yield return new WaitForFixedUpdate(); // Bir frame bekle
        
        if (agent != null && agent.enabled && !agent.isOnNavMesh)
        {
            // NavMesh üzerinde geçerli bir pozisyon bul
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
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
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator PlayRandomSoundLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(waitTime);

            if (randomSounds.Length > 0 && audioSource != null && enabled)
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