using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    private bool isLocked = true;
    private SceneChange sceneChange;

    void Start()
    {
        LockDoor();

        sceneChange = FindObjectOfType<SceneChange>();  
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLocked)
        {   
            sceneChange.BeforeCustomerSceneChange();
        }
    }

        public void LockDoor()
    {
        isLocked = true;

    }
}
