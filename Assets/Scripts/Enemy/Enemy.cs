using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] GameObject lootPrefab; // Loot prefab'ı

    void Start()
    {
        EnemyManager.instance.RegisterEnemy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Loot spawn işlemi
        SpawnLoot();

        Destroy(gameObject);
    }

    void SpawnLoot()
    {
        if (lootPrefab != null)
        {
            // Loot'u düşmanın pozisyonunda spawn et
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnDestroy()
    {
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.EnemyDefeated(gameObject);
        }
    }
}
