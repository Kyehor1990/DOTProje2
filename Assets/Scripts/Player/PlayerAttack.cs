using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;
    private float lastAttackTime;
    private Vector2 attackDirection;

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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);
            hit.collider.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)attackDirection * attackRange);
    }
}