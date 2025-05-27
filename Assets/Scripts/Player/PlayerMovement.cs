using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    PlayerAttack _playerAttack;


    void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        movement.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        movement.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        movement = movement.normalized;

        if (movement != Vector2.zero && _playerAttack?.isAttacking == false)
        {
            if (movement.x > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (movement.x < 0)
                transform.rotation = Quaternion.Euler(0, 0, 180);
            else if (movement.y > 0)
                transform.rotation = Quaternion.Euler(0, 0, 90);
            else if (movement.y < 0)
                transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

void FixedUpdate()
{
    if (_playerAttack != null && _playerAttack.isAttacking)
    {
        rb.velocity = Vector2.zero;
    }
    else
    {
        rb.velocity = movement * moveSpeed;
    }
}

}