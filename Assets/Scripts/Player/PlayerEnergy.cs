using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public static PlayerEnergy instance;
    public int maxEnergy = 100;
    public int currentEnergy;



    void Awake()
    {
        if (instance == null) instance = this;
        currentEnergy = maxEnergy;
    }

    public void UseEnergy(int amount)
    {
        Debug.Log("Enerji kullanıldı: " + amount);
        currentEnergy -= amount;
    }

    public void ForceExitDungeon()
    {
        Debug.Log("Enerjin bitti! Zindandan çıkıyorsun...");
    }
    
    public void UpgradeEnergy(int amount)
    {
        maxEnergy += amount;
    }
}
