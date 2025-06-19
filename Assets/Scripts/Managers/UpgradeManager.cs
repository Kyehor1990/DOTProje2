using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public StockManager stockManager;
    public PlayerEnergy playerEnergy;
    public PlayerAttack playerAttack;
    public PlayerMovement playerMovement;
    public InventoryManager inventoryManager;
    public CustomerManager customerManager;
    
    [Header("Slot Upgrade UI References")]
    public UnityEngine.UI.Button singleSlotUpgradeButton;
    public UnityEngine.UI.Button multipleSlotUpgradeButton;
    public UnityEngine.UI.Text singleSlotPriceText;
    public UnityEngine.UI.Text multipleSlotPriceText;
    public UnityEngine.UI.Text slotInfoText;
    
    [Header("Multiple Slot Purchase Settings")]
    public int multipleSlotCount = 5; // Toplu satın almada kaç slot alınacak

    void Start()
    {
        UpdateSlotUpgradeUI();
    }

    void Update()
    {
        UpdateSlotUpgradeUI();
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
        int multiplePrice = singlePrice * multipleSlotCount;
        
        // Tek slot butonu
        if (singleSlotUpgradeButton != null)
        {
            singleSlotUpgradeButton.interactable = canUpgrade && stockManager.playerMoney >= singlePrice;
        }
        
        if (singleSlotPriceText != null)
        {
            if (canUpgrade)
            {
                singleSlotPriceText.text = $"${singlePrice}";
            }
            else
            {
                singleSlotPriceText.text = "MAX";
            }
        }
        
        
        // Slot bilgi metni
        if (slotInfoText != null)
        {
            int unlockedSlots = inventoryManager.GetUnlockedSlotCount();
            int totalSlots = inventoryManager.GetTotalSlotCount();
            
            if (canUpgrade)
            {
                slotInfoText.text = $"Slots: {unlockedSlots}/{totalSlots}";
            }
            else
            {
                slotInfoText.text = $"All Slots Unlocked ({totalSlots}/{totalSlots})";
            }
        }
    }
}