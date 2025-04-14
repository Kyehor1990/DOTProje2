using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
     public bool Dungeon = true;
     public Transform cameraTransform;
     public GameObject player;
     public PlayerHealth playerHealth;
     public Vector3 targetPosition;

     
     public void CustomerSceneChange()
     {
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
