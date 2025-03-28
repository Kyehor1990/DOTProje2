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
        if (Input.GetKeyDown(KeyCode.C) && Dungeon && playerHealth.currentHealth > 0) 
        {

            Debug.Log("Dungeon'dan çikiş yapildi.");
            if (cameraTransform != null)
            {
                cameraTransform.position = targetPosition;
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

            Dungeon = true;
        }
    }
}
