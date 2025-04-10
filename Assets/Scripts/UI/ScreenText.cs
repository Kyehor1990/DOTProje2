using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenText : MonoBehaviour
{

    public Item kisir;

    public Item russalatasi;
    public TextMeshProUGUI textMeshPro;

    public StockManager stockManager;
    void Start()
    {
        
    }

    void Update()
    {
         textMeshPro.text = "Kısır: " + kisir.stock.ToString() + "\n" + "Russalatasi: " + russalatasi.stock.ToString() + 
         "\n" + "Money: " + stockManager.playerMoney.ToString();
    }
}
