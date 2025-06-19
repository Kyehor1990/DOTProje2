using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenText : MonoBehaviour
{

    public Item kisir;

    public Item patates;
    public Item turşu;
    public Item sosis;
    public TextMeshProUGUI textMeshPro;

    public StockManager stockManager;
    void Start()
    {
        
    }

    void Update()
    {
         textMeshPro.text = "Kısır: " + kisir.stock.ToString() + "\n" + "Patates: " + patates.stock.ToString() + "\n" + "Turşu:" + turşu.stock.ToString() + "\n" + "Sosis: " + sosis.stock.ToString() + "\n" +
         "\n" + "Money: " + stockManager.playerMoney.ToString();
    }
}
