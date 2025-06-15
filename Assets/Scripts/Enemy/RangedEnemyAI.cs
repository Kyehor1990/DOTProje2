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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("Attack");
    }

    // Bu fonksiyonu animasyonun ortasında Animation Event olarak çağır
    public void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(direction);
        }
    }
}
