using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public PlayerHealth playerHealth;

    void Update()
    {
        textMeshPro.text = "Can: " + playerHealth.currentHealth.ToString();
    }

}
