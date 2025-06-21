using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFeedback : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float knockbackForce = 5f;
    public float colorChangeDuration = 0.2f;
    public float disableAIDuration = 0.5f;

    public bool isKinematic = true;

    private Rigidbody2D rb;
    private EnemyAI aiScript;
    private NavMeshAgent navMeshAgent;
    private Vector3 lastValidPosition; // Son geçerli pozisyonu kaydet

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        rb = GetComponent<Rigidbody2D>();
        if (isKinematic)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        aiScript = GetComponent<EnemyAI>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        lastValidPosition = transform.position;
    }

    public void TakeHit(Vector2 hitDirection)
    {
        StopAllCoroutines();
        StartCoroutine(FlashWhite());

        // NavMeshAgent'ı önce durdur
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.enabled = false;
        }

        // AI'yi devre dışı bırak
        if (aiScript != null)
        {
            aiScript.enabled = false;
        }

        // Knockback uygula
        if (rb != null)
        {
            if (isKinematic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            rb.velocity = Vector2.zero;
            rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);
        }

        // AI'yi yeniden etkinleştir
        Invoke(nameof(EnableAI), disableAIDuration);
        Invoke(nameof(ResetRigidbody), disableAIDuration);
    }

    void EnableAI()
    {
        if (aiScript != null)
        {
            aiScript.enabled = true;
        }

        // NavMeshAgent'ı yeniden etkinleştir
        if (navMeshAgent != null)
        {
            // Pozisyonu NavMesh üzerinde geçerli bir yere ayarla
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
            else
            {
                // Eğer geçerli pozisyon bulunamazsa, son geçerli pozisyona geri dön
                transform.position = lastValidPosition;
            }

            navMeshAgent.enabled = true;
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = Vector3.zero;
        }
    }

    void ResetRigidbody()
    {
        if (rb != null && isKinematic)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        
        // Son geçerli pozisyonu güncelle
        lastValidPosition = transform.position;
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = originalColor;
    }

    void Update()
    {
        // NavMeshAgent aktifse ve hareket ediyorsa pozisyonu kaydet
        if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            lastValidPosition = transform.position;
        }
    }
}