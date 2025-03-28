using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 targetPosition;
    public GameObject player;
    
    public bool Dungeon = true;

    public PlayerHealth playerHealth;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && Dungeon) 
        {

            Debug.Log("Dungeon'dan çikiş yapildi.");
            if (cameraTransform != null)
            {
                cameraTransform.position = targetPosition;
            }

            if (player != null)
            {
                player.SetActive(false);
            }
            playerHealth.currentHealth = playerHealth.maxHealth;
            Dungeon = false;

        }
        else if (Input.GetKeyDown(KeyCode.C) && !Dungeon)
        {
            Debug.Log("Dungeon'a giriş yapildi.");

            if (cameraTransform != null)
            {
                cameraTransform.position = new Vector3(3.21f, 0, -10);
            }

            if (player != null)
            {
                player.SetActive(true);
            }

            Dungeon = true;
        }
    }
}
