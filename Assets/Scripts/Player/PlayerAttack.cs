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

    [SerializeField] private Animator animator;

    void Update()
    {
        if (isAttacking || isOnCooldown)
            return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            attackDirection = Vector2.up;
            SetAttackVisual(Vector2.up);
            StartCoroutine(AttackRoutine());
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            attackDirection = Vector2.down;
            SetAttackVisual(Vector2.down);
            StartCoroutine(AttackRoutine());
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            attackDirection = Vector2.left;
            SetAttackVisual(Vector2.left);
            StartCoroutine(AttackRoutine());
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            attackDirection = Vector2.right;
            SetAttackVisual(Vector2.right);
            StartCoroutine(AttackRoutine());
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
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        isOnCooldown = true;

        yield return new WaitForSeconds(attackCooldown);

        isOnCooldown = false;
    }

    public void ApplyAttackDamage() // Called by animation event
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);

            Vector2 knockbackDir = enemy.transform.position - transform.position;
            enemy.GetComponent<EnemyFeedback>().TakeHit(knockbackDir);
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
}
