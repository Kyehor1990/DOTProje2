using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    int dayCount = 0;
    public TextMeshProUGUI textMeshPro;

    public void DayCountIncrease()
    {
        dayCount++;
        Debug.Log("Day: " + dayCount);
    }

    void Update()
    {
        textMeshPro.text = "Day: " + dayCount.ToString();
    }
}
