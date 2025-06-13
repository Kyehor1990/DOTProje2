using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] int damage = 1;
    [SerializeField] float lifetime = 5f;

    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
