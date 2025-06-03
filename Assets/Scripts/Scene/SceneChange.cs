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
     public GameObject UpgradePanel;

     public void CustomerSceneChange()
     {
            if (Dungeon && playerHealth.currentHealth > 0) 
            {
                Debug.Log("Dungeon'dan çikiş yapildi.");
                if (cameraTransform != null)
                {
                    cameraTransform.position = targetPosition;
                }
                destroyDungeon.DungeonDestroy();
                player.SetActive(false);
                StartCoroutine(customerManager.CustomerRoutine());
                Dungeon = false;

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
                player.SetActive(true);
                playerHealth.currentHealth = playerHealth.maxHealth;
                playerEnergy.currentEnergy = playerEnergy.maxEnergy;

                customerManager.ResetCustomerCount();
                destroyDungeon.DungeonCreate();
                dayManager.DayCountIncrease();
                Dungeon = true;
                dungeonButton.gameObject.SetActive(false);
                UpgradePanel.SetActive(false);
            }
     }
}
