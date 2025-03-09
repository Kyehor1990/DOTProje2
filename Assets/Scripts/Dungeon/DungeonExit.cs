using UnityEngine;

public class DungeonExit : MonoBehaviour
{
        private bool isLocked = true;

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
        }
    }

        public void LockDoor()
    {
        isLocked = true;

    }
}
