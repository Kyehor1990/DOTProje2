using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator;
    private PlayerAttack _playerAttack;

    float speed2;
    [SerializeField] private AudioClip[] footstepSounds; // 7 farklı ses
    public AudioSource audioSource;
    private int lastPlayedIndex = -1;
    private float footstepInterval = 0.4f; // her adım arası süre
    private float footstepTimer = 0f;


    void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        movement.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        movement = movement.normalized;

        animator.SetBool("IsMovingX", Mathf.Abs(movement.x) > 0.01f);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Flip sprite based on movement ONLY if not attacking
        if (!_playerAttack.isAttacking && movement.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = movement.x > 0 ? 1 : -1;
            transform.localScale = scale;
        }

        if (!_playerAttack.isAttacking && movement.sqrMagnitude > 0.01f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayRandomFootstepSound();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f; // durunca sıfırla
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

    public void upgradeSpeed()
    {
        speed2 = moveSpeed * 0.10f;
        moveSpeed += speed2;
    }
    
    private void PlayRandomFootstepSound()
{
    if (footstepSounds.Length == 0) return;

    int newIndex;
    do
    {
        newIndex = Random.Range(0, footstepSounds.Length);
    } while (newIndex == lastPlayedIndex && footstepSounds.Length > 1);

    lastPlayedIndex = newIndex;
    audioSource.PlayOneShot(footstepSounds[newIndex]);
}

}
