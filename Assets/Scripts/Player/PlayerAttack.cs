using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float attackDuration = 0.3f;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    private bool isOnCooldown = false;
    public bool isAttacking = false;

    private Vector2 attackDirection;
    public Transform attackPoint;

    void Update()
    {
        if (isAttacking || isOnCooldown)
            return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            attackDirection = Vector2.up;
            RotatePlayer(attackDirection);
            StartCoroutine(AttackRoutine());
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            attackDirection = Vector2.down;
            RotatePlayer(attackDirection);
            StartCoroutine(AttackRoutine());
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            attackDirection = Vector2.left;
            RotatePlayer(attackDirection);
            StartCoroutine(AttackRoutine());
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            attackDirection = Vector2.right;
            RotatePlayer(attackDirection);
            StartCoroutine(AttackRoutine());
        }
    }

    void RotatePlayer(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // Burada animasyon tetiklenebilir
        // animator.SetTrigger("Attack");

        // Hasarı bu anda uygula
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Vector2 knockbackDir = enemy.transform.position - transform.position;
            enemy.GetComponent<EnemyFeedback>().TakeHit(knockbackDir);
        }

        yield return new WaitForSeconds(attackDuration); // Saldırı süresi

        isAttacking = false;
        isOnCooldown = true;

        yield return new WaitForSeconds(attackCooldown); // Cooldown

        isOnCooldown = false;
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
