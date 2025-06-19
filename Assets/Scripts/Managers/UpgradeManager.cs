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
}