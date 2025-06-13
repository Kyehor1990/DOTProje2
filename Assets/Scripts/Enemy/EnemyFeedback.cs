using System.Collections;
using UnityEngine;

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
    }

    public void TakeHit(Vector2 hitDirection)
    {
        StopAllCoroutines();
        StartCoroutine(FlashWhite());

        if (rb != null)
        {
            if (isKinematic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            rb.velocity = Vector2.zero;
            rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);
        }

        if (aiScript != null)
        {
            aiScript.enabled = false;
            Invoke(nameof(EnableAI), disableAIDuration);
        }

        Invoke(nameof(ResetRigidbody), disableAIDuration);
    }

    void EnableAI()
    {
        if (aiScript != null)
        {
            aiScript.enabled = true;
        }
    }

    void ResetRigidbody()
    {
        if (rb != null && isKinematic)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = originalColor;
    }
}
