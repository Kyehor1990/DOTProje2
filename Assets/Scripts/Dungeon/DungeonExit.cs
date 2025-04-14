using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    private bool isLocked = true;
    public SceneChange sceneChange;

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
            sceneChange.CustomerSceneChange();
        }
    }

        public void LockDoor()
    {
        isLocked = true;

    }
}
