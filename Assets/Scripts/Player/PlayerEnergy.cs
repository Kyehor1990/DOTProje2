using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public static PlayerEnergy instance;
    public int maxEnergy = 100;
    private int currentEnergy;

    void Awake()
    {
        if (instance == null) instance = this;
        currentEnergy = maxEnergy;
    }

    public void UseEnergy(int amount)
    {
        currentEnergy -= amount;
        if (currentEnergy <= 0)
        {
            ForceExitDungeon();
        }
    }

    void ForceExitDungeon()
    {
        Debug.Log("Enerjin bitti! Zindandan çıkıyorsun...");
    }
}
