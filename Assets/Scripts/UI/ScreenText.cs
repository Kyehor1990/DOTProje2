using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScreenText : MonoBehaviour
{

    public Item kisir;

    public Item patates;
    public Item turşu;
    public Item sosis;
    public TextMeshProUGUI textMeshPro;
    public GameObject can;
    public GameObject customerPanel;
    public TextMeshProUGUI energyText;

    public StockManager stockManager;

    public PlayerEnergy playerEnergy;

    void Update()
    {
        textMeshPro.text = "Kısır: " + kisir.stock.ToString() + "\n" + "Patates: " + patates.stock.ToString() + "\n" + "Turşu:" + turşu.stock.ToString() + "\n" + "Sosis: " + sosis.stock.ToString() + "\n" +
        "\n" + "Money: " + stockManager.playerMoney.ToString();

        energyText.text = "Energy: " + playerEnergy.currentEnergy.ToString() + "/" + playerEnergy.maxEnergy.ToString();
    }

    public void CustomerText()
    {
        textMeshPro.gameObject.SetActive(!textMeshPro.gameObject.activeSelf);
        customerPanel.SetActive(!customerPanel.activeSelf);
    }

    public void DungeonText()
    {
        can.SetActive(!can.activeSelf);
        energyText.gameObject.SetActive(!energyText.gameObject.activeSelf);
    }   
}
