using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    int dayCount = 1;
    public TextMeshProUGUI textMeshPro;
    public UpgradeManager upgradeManager;

    public void DayCountIncrease()
    {
        dayCount++;
        upgradeManager.kiraSayaç++;
        Debug.Log("Day: " + dayCount);
        if (dayCount % 7 == 0)
        {
            Zam();
        }
    }

    void Update()
    {
        textMeshPro.text = "Day: " + dayCount.ToString();


    }

    void Zam()
    {
        if (dayCount % 7 == 0)
        {
            upgradeManager.kira += 5;
        } 
    }
}
