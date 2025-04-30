using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public StockManager stockManager;
    public	void UpgradeHealthButton()
    {
        playerHealth.UpgradeHealth(1);
        stockManager.BuyUpgrade(5);
    }
}
