using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    [Header("Saldırı Ayarları")]
    [SerializeField] float attackRange = 5f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;

    private Transform player;
    private float lastAttackTime;
    private Animator animator;
    [SerializeField] private AudioClip prepareSound;
[SerializeField] private AudioClip shootSound;
private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0.05f;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                StartAttack();
            }
        }

        // Oyuncuya dön
        Vector3 scale = transform.localScale;
        scale.x = player.position.x > transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void StartAttack()
    {
        if (prepareSound != null && audioSource != null)
    {
        audioSource.PlayOneShot(prepareSound);
    }
        animator.SetTrigger("Attack");
    }

    // Bu fonksiyonu animasyonun ortasında Animation Event olarak çağır
    public void ShootProjectile()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        if (projectilePrefab != null && firePoint != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(direction);
        }
    }
}
