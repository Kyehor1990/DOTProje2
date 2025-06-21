using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float attackDuration = 0.3f;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    public Transform attackPoint;
    public bool isAttacking = false;

    private Vector2 attackDirection;
    private bool isOnCooldown = false;
    private Coroutine currentAttackCoroutine; // Aktif coroutine referansı

    [SerializeField] private Animator animator;

    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] AudioSource audioSource;
    private int lastPlayedIndex = -1;

    void Start()
    {
        // Başlangıçta durumları sıfırla
        isAttacking = false;
        isOnCooldown = false;
    }

    void Update()
    {
        // Debug için durumları konsola yazdır (geliştirme sırasında)
        // Debug.Log($"IsAttacking: {isAttacking}, IsOnCooldown: {isOnCooldown}");

        if (isAttacking || isOnCooldown)
            return;

        Vector2 inputDirection = Vector2.zero;
        bool hasInput = false;

        // Input kontrolü daha temiz hale getirildi
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputDirection = Vector2.up;
            hasInput = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputDirection = Vector2.down;
            hasInput = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputDirection = Vector2.left;
            hasInput = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputDirection = Vector2.right;
            hasInput = true;
        }

        if (hasInput)
        {
            // Önceki coroutine'i durdur (güvenlik için)
            if (currentAttackCoroutine != null)
            {
                StopCoroutine(currentAttackCoroutine);
            }

            attackDirection = inputDirection;
            SetAttackVisual(inputDirection);
            currentAttackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    void SetAttackVisual(Vector2 direction)
    {
        // Flip player if horizontal
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction.x > 0 ? 1 : -1;
            transform.localScale = scale;
        }

        // Set animator direction
        int dirIndex = direction == Vector2.down ? 0 :
                       direction == Vector2.up ? 1 :
                       direction == Vector2.left ? 2 :
                       3; // right
        animator.SetInteger("AttackDirection", dirIndex);

        // Calculate world offset
        Vector2 offset = Vector2.zero;
        float distance = 0.5f;

        if (direction == Vector2.up)
            offset = new Vector2(0, distance);
        else if (direction == Vector2.down)
            offset = new Vector2(0, -distance);
        else if (direction == Vector2.left)
            offset = new Vector2(-distance, 0);
        else if (direction == Vector2.right)
            offset = new Vector2(distance, 0);

        // Use world-space position so it's not affected by flipping
        attackPoint.position = transform.position + (Vector3)offset;
    }

    IEnumerator AttackRoutine()
    {
        // Durumları açıkça ayarla
        isAttacking = true;
        isOnCooldown = false;

        // Animator null kontrolü
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Attack duration bekle
        yield return new WaitForSeconds(attackDuration);

        // Attack bittiği zaman durumu güncelle
        isAttacking = false;
        isOnCooldown = true;

        // Cooldown bekle
        yield return new WaitForSeconds(attackCooldown);

        // Cooldown bittiği zaman durumu güncelle
        isOnCooldown = false;
        currentAttackCoroutine = null; // Referansı temizle
    }

    public void ApplyAttackDamage() // Called by animation event
    {
        // Sadece saldırı sırasında hasar ver
        if (!isAttacking)
            return;

        PlayRandomAttackSound();
        
        // AttackPoint null kontrolü
        if (attackPoint == null)
        {
            Debug.LogWarning("AttackPoint is null!");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy == null) continue;

            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            EnemyFeedback feedbackComponent = enemy.GetComponent<EnemyFeedback>();

            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(attackDamage);
            }

            if (feedbackComponent != null)
            {
                Vector2 knockbackDir = (enemy.transform.position - transform.position).normalized;
                feedbackComponent.TakeHit(knockbackDir);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    public void UpgradeAttack(int amount)
    {
        attackDamage += amount;
    }

    private void PlayRandomAttackSound()
    {
        if (attackSounds.Length == 0 || audioSource == null)
            return;

        int newIndex;

        // Aynı ses üst üste çalmasın
        do
        {
            newIndex = Random.Range(0, attackSounds.Length);
        } while (attackSounds.Length > 1 && newIndex == lastPlayedIndex);

        lastPlayedIndex = newIndex;
        AudioClip selectedClip = attackSounds[newIndex];

        if (selectedClip != null)
        {
            audioSource.PlayOneShot(selectedClip);
        }
    }

    // Debug için manuel reset fonksiyonu
    [ContextMenu("Reset Attack States")]
    public void ResetAttackStates()
    {
        if (currentAttackCoroutine != null)
        {
            StopCoroutine(currentAttackCoroutine);
            currentAttackCoroutine = null;
        }
        isAttacking = false;
        isOnCooldown = false;
        Debug.Log("Attack states reset!");
    }

    // GameObject deaktif edildiğinde coroutine'leri temizle
    void OnDisable()
    {
        if (currentAttackCoroutine != null)
        {
            StopCoroutine(currentAttackCoroutine);
            currentAttackCoroutine = null;
        }
        isAttacking = false;
        isOnCooldown = false;
    }
}