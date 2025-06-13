using System.Collections;
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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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
                ShootProjectile();
            }
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            Vector2 direction = (player.position - firePoint.position).normalized;
            projectile.GetComponent<Projectile>().Initialize(direction);
        }
    }
}
