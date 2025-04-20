using System.Collections;
using UnityEngine;

public class EnemyFeedback : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float knockbackForce = 5f;
    public float colorChangeDuration = 0.2f;
    private Rigidbody2D rb;
    private EnemyAI aiScript;
    public float disableAIDuration = 0.5f;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        rb = GetComponent<Rigidbody2D>();
        aiScript = GetComponent<EnemyAI>();
    }

    public void TakeHit(Vector2 hitDirection)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);

        if (aiScript != null)
        {
            aiScript.enabled = false;
            Invoke(nameof(EnableAI), disableAIDuration);
        }

        StopAllCoroutines();
        StartCoroutine(FlashWhite());
    }

    void EnableAI()
    {
        if (aiScript != null)
        {
            aiScript.enabled = true;
        }
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = originalColor;
    }
}
