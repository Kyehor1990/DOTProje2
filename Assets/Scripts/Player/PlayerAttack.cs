using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange;
    public int attackDamage = 1;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;
    private float lastAttackTime;
    private Vector2 attackDirection;

    public Transform attackPoint;

    public bool isAttacking = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            attackDirection = Vector2.up;
            RotatePlayer(attackDirection);
            TryAttack();
            return;

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            attackDirection = Vector2.down;
            RotatePlayer(attackDirection);
            TryAttack();
            return;

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            attackDirection = Vector2.left;
            RotatePlayer(attackDirection);
            TryAttack();
            return;

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            attackDirection = Vector2.right;
            RotatePlayer(attackDirection);
            TryAttack();
            return;
        }

        isAttacking = false;

    }

    void RotatePlayer(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        isAttacking = true;

        // Play attack animation (you can trigger this via Animator)
        // animator.SetTrigger("Attack");

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}