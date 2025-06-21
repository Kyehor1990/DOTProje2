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
    [SerializeField] Transform startTransform;
    public float duration = 1f;

    public ScreenText screenText;
    public PickupItem pickupItem;
    public EnemyManager enemyManager;

    void Start()
    {
        Dungeon = true;
        DungeonMusicManager.instance?.PlayDungeonMusic();
        CustomerMusicManager.instance?.StopCustomerMusic();
    }
    public void CustomerSceneChange()
    {
        if (!Dungeon)
        {
            Debug.Log("Dungeon'dan çikiş yapildi.");
            player2.SetActive(false);

            if (cameraTransform != null)
            {
                cameraTransform.position = targetPosition;
            }
            StartCoroutine(customerManager.CustomerRoutine());
            Dungeon = false;
            screenText.CustomerText();

        }
    }

    public void DungeonSceneChange()
    {
        if (!Dungeon)
        {
            Debug.Log("Customer'dan çikiş yapildi.");
            player2.SetActive(false);

            if (cameraTransform != null)
            {
                cameraTransform.position = new Vector3(3.21f, 0, -10);
            }
            player.SetActive(true);
            playerHealth.currentHealth = playerHealth.maxHealth;
            playerEnergy.currentEnergy = playerEnergy.maxEnergy;

            destroyDungeon.DungeonCreate();
            dayManager.DayCountIncrease();
            playerHealth.isInvincible = false;
            Dungeon = true;
            DungeonMusicManager.instance?.PlayDungeonMusic();
            CustomerMusicManager.instance?.StopCustomerMusic();
            screenText.DungeonText();


        }
    }

    public void BeforeDungeonSceneChange()
{
        if (!Dungeon)
        {
            screenText.CustomerText();
            player2.SetActive(true);
            player2.transform.localScale = new Vector3(1f, 1, 1);
            player2.transform.position = startTransform.position;

            if (cameraTransform != null)
            {
                cameraTransform.position = new Vector3(3.7f, -75f, -10f);
            }
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
        if (Dungeon)
        {
            Dungeon = false;
            DungeonMusicManager.instance?.StopDungeonMusic(); 
            CustomerMusicManager.instance?.PlayCustomerMusic();
            pickupItem.ResetPickupItems();
            screenText.DungeonText();

            player2.SetActive(true);
            player2.transform.localScale = new Vector3(-1f, 1, 1);
            player2.transform.position = targetTransform.position;

            if (cameraTransform != null)
            {
                cameraTransform.position = new Vector3(3.7f, -75f, -10f);
            }
            destroyDungeon.DungeonDestroy();
            player.SetActive(false);

            target = startTransform.position;
            player2.transform.DOMoveX(target.x, duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(CustomerSceneChange);


        }
    }
    
        void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && enemyManager.dungeonExit && Dungeon)
        {
            BeforeCustomerSceneChange();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            BeforeDungeonSceneChange();
        }
    }
}
