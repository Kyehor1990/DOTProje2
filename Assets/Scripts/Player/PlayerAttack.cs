using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    private float lastAttackTime;
    private Vector2 attackDirection;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            attackDirection = Vector2.up;
            Attack();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            attackDirection = Vector2.down;
            Attack();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            attackDirection = Vector2.left;
            Attack();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            attackDirection = Vector2.right;
            Attack();
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        // Update last attack time
        lastAttackTime = Time.time;

        // Play attack animation (you can trigger this via Animator)
        // animator.SetTrigger("Attack");

        // Detect enemies in the attack range
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, enemyLayer);

        // If an enemy is hit, deal damage
        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);
            // Assuming the enemy has a script with a TakeDamage method
            hit.collider.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    // Visualize the attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)attackDirection * attackRange);
    }
}