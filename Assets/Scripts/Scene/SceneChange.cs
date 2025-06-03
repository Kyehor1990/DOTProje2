using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


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
    [SerializeField] GameObject player2;
    private Vector3 target;
    [SerializeField] Transform targetTransform;
    public float duration = 1f;

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

            destroyDungeon.DungeonCreate();
            dayManager.DayCountIncrease();
            Dungeon = true;

        }
    }

    public void BeforeDungeonSceneChange()
{
    if (!Dungeon)
    {
        player2.SetActive(true);

        if (cameraTransform != null)
        {
            cameraTransform.position = new Vector3(3.7f, -75f, -10f);
        };
            dungeonButton.gameObject.SetActive(false);
            UpgradePanel.SetActive(false);
            customerManager.ResetCustomerCount();

            target = targetTransform.position;
            player2.transform.DOMoveX(target.x, duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(DungeonSceneChange);
    }
}

    public void BeforeCustomerSceneChange()
    {

    }
    
        void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CustomerSceneChange();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            BeforeDungeonSceneChange();
        }
    }
}
