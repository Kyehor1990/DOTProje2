using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
     public bool Dungeon = true;
     public Transform cameraTransform;
     public GameObject player;
     public PlayerHealth playerHealth;
     public Vector3 targetPosition;

     public DestroyDungeon destroyDungeon;

     public PlayerEnergy playerEnergy;

     public CustomerManager customerManager;

     public DayManager dayManager;
     public Button dungeonButton;

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

                destroyDungeon.DungeonDestroy();
                StartCoroutine(customerManager.CustomerRoutine());
                Dungeon = false;

                playerEnergy.currentEnergy = playerEnergy.maxEnergy;

            }
     }

     public void DungeonSceneChange()
     {
            if (!Dungeon && playerHealth.currentHealth > 0) 
            {
                Debug.Log("Customer'dan çikiş yapildi.");
                if (cameraTransform != null)
                {
                    cameraTransform.position = new Vector3(3.21f, 0, -10);
                }

                destroyDungeon.DungeonCreate();
                dayManager.DayCountIncrease();
                Dungeon = true;
                dungeonButton.gameObject.SetActive(false);
            }
     }
}
