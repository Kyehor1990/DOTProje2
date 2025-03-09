using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 3;

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
        Destroy(gameObject);
    }

    void OnDestroy()
{
    if (EnemyManager.instance != null)
    {
        EnemyManager.instance.EnemyDefeated(gameObject);
    }
}
}