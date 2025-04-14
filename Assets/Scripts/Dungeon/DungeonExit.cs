using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    private bool isLocked = true;

    public Transform cameraTransform;
    public Vector3 targetPosition;
    public GameObject player;
    
    public bool Dungeon = true;

    public PlayerHealth playerHealth;

    void Start()
    {
        LockDoor();    
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLocked)
        {
            Debug.Log("Zindandan çıktın!");
            
            if (Dungeon && playerHealth.currentHealth > 0) 
            {
                Debug.Log("Dungeon'dan çikiş yapildi.");
                if (cameraTransform != null)
                {
                    cameraTransform.position = targetPosition;
                }

                playerHealth.currentHealth = playerHealth.maxHealth;
                Dungeon = false;
            }
        }
    }

        public void LockDoor()
    {
        isLocked = true;

    }
}
