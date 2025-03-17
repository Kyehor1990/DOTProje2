using System.Collections.Generic;
using UnityEngine;

public class StockManager : MonoBehaviour
{
    public List<Item> items;
    public int playerMoney = 0;

    public bool ProcessOrder(string requestedItem)
    {
        foreach (Item item in items)
        {
            if (item.itemName == requestedItem)
            {
                if (item.stock > 0)
                {
                    item.stock--;
                    playerMoney += item.price;
                    Debug.Log($"Sold {requestedItem}! New stock: {item.stock}. Player Money: {playerMoney}");
                    return true;
                }
                else
                {
                    Debug.Log($"Out of stock: {requestedItem}");
                    return false;
                }
            }
        }
        Debug.Log($"Item {requestedItem} not found in stock.");
        return false;
    }
}
