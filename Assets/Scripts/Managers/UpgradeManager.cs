using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public StockManager stockManager;
    public PlayerEnergy playerEnergy;
    public PlayerAttack playerAttack;
    public PlayerMovement playerMovement;
    public InventoryManager inventoryManager;
    public CustomerManager customerManager;

    public TextMeshProUGUI kiraText;
    public GameObject kiraButton;

    public int kiraSayaç = 0;

    public int kira = 10;
    public GameObject Kovuldun;

    public GameObject player;



    void Start()
    {
        Time.timeScale = 1f; // Oyunu başlat
        Kovuldun.SetActive(false);
        UpdateSlotUpgradeUI();
    }

    void Update()
    {
        UpdateSlotUpgradeUI();

        kiraText.text = "Kira: " + kira.ToString();

        if (kiraSayaç >= 3)
        {
            Kovuldun.SetActive(true);
            Destroy(player);
            Time.timeScale = 0f; // Oyunu durdur
            
        }
    }

    public void UpgradeHealthButton()
    {
        if (stockManager.playerMoney >= 50)
        {
            playerHealth.UpgradeHealth(20);
            stockManager.BuyUpgrade(50);
        }
    }

    public void UpgradeEnergyButton()
    {
        if (stockManager.playerMoney >= 50)
        {
            playerEnergy.UpgradeEnergy(1);
            stockManager.BuyUpgrade(50);
        }
    }

    public void UpgradeAttackButton()
    {
        if (stockManager.playerMoney >= 60)
        {
            playerAttack.UpgradeAttack(2);
            stockManager.BuyUpgrade(60);
        }
    }

    public void UpgradeSpeedButton()
    {
        if (stockManager.playerMoney >= 40)
        {
            playerMovement.upgradeSpeed();
            stockManager.BuyUpgrade(40);
        }
    }

    public void UpgradeCustomerButton()
    {
        if (stockManager.playerMoney >= 70)
        {
            customerManager.UpgradeCustomer();
            stockManager.BuyUpgrade(70);
        }
    }

    // YENİ: Tek slot yükseltme butonu
    public void UpgradeSingleSlotButton()
    {
        if (!inventoryManager.CanUpgradeSlots())
        {
            Debug.Log("All inventory slots are already unlocked!");
            return;
        }

        int price = inventoryManager.GetSlotUpgradePrice();

        if (stockManager.playerMoney >= price)
        {
            bool success = inventoryManager.UpgradeSlot(stockManager.playerMoney);
            if (success)
            {
                stockManager.BuyUpgrade(price);
                Debug.Log($"1 inventory slot unlocked for {price} coins!");
            }
        }
        else
        {
            Debug.Log($"Not enough money! Need {price}, have {stockManager.playerMoney}");
        }
    }


    // UI güncellemesi
    void UpdateSlotUpgradeUI()
    {
        bool canUpgrade = inventoryManager.CanUpgradeSlots();
        int singlePrice = inventoryManager.GetSlotUpgradePrice();
    }

    public void PayRent()
    {
        if (stockManager.playerMoney >= kira)
        {
            kiraSayaç--;
            stockManager.playerMoney -= kira;
            kiraButton.SetActive(false);
        }
    }

}