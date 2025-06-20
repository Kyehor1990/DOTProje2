using System.Collections.Generic;
using UnityEngine;

public class StockManager : MonoBehaviour
{
    public List<Item> items;
    public int playerMoney = 0;

    // Tek ürün siparişi için (geriye dönük uyumluluk)
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
                    Debug.Log($"Satıldı {requestedItem}! Yeni stok: {item.stock}. Oyuncu Parası: {playerMoney}");
                    return true;
                }
                else
                {
                    Debug.Log($"Stokta yok: {requestedItem}");
                    return false;
                }
            }
        }
        Debug.Log($"Ürün {requestedItem} stokta bulunamadı.");
        return false;
    }

    // Birden fazla ürün siparişi için
    public bool ProcessOrder(CustomerManager.CustomerOrder order)
    {
        Debug.Log("=== SİPARİŞ İŞLEME BAŞLADI ===");

        // Önce tüm ürünlerin mevcut olup olmadığını kontrol et
        Dictionary<Item, int> requiredItems = new Dictionary<Item, int>(); // Item ve kaç adet gerektiği
        int totalPrice = 0;

        // Her istenen ürün için kontrol yap
        foreach (string requestedItemName in order.requestedItems)
        {
            Debug.Log($"Aranan ürün: '{requestedItemName}'");
            Item foundItem = null;

            // Ürünü bul
            foreach (Item item in items)
            {
                Debug.Log($"Karşılaştırma: '{requestedItemName}' vs '{item.itemName}'");
                if (item.itemName.Trim() == requestedItemName.Trim())
                {
                    foundItem = item;
                    Debug.Log($"Ürün bulundu: {foundItem.itemName} (Stok: {foundItem.stock})");
                    break;
                }
            }

            // Ürün bulunamazsa sipariş başarısız
            if (foundItem == null)
            {
                Debug.Log($"HATA: Ürün '{requestedItemName}' stokta bulunamadı!");
                Debug.Log("=== Mevcut tüm ürünler ===");
                foreach (Item item in items)
                {
                    Debug.Log($"- '{item.itemName}' (Stok: {item.stock})");
                }
                return false;
            }

            // Aynı üründen kaç adet isteniyor sayalım
            if (requiredItems.ContainsKey(foundItem))
            {
                requiredItems[foundItem]++;
            }
            else
            {
                requiredItems.Add(foundItem, 1);
            }

            totalPrice += foundItem.price;
        }

        // Tüm ürünlerin yeterli stokta olup olmadığını kontrol et
        foreach (var kvp in requiredItems)
        {
            Item item = kvp.Key;
            int requiredAmount = kvp.Value;

            if (item.stock < requiredAmount)
            {
                Debug.Log($"HATA: Yetersiz stok: {item.itemName} (Gerekli: {requiredAmount}, Mevcut: {item.stock})");
                return false;
            }
        }

        // Tüm ürünler yeterli miktarda mevcutsa, satışı gerçekleştir
        Debug.Log("=== SATIŞ GERÇEKLEŞTİRİLİYOR ===");
        foreach (var kvp in requiredItems)
        {
            Item item = kvp.Key;
            int amountToSell = kvp.Value;

            item.stock -= amountToSell;
            Debug.Log($"✓ Satıldı {item.itemName} x{amountToSell}! Yeni stok: {item.stock}");
        }

        playerMoney += totalPrice;
        Debug.Log($"✓ Toplam satış: {totalPrice} TL. Oyuncu Parası: {playerMoney} TL");

        order.isOrderComplete = true;
        return true;
    }

    public void BuyUpgrade(int amount)
    {
        playerMoney -= amount;
        Debug.Log($"Upgrade satın alındı: {amount}. Oyuncu Parası: {playerMoney}");
    }

    // Belirli bir ürünün stokunu kontrol etmek için yardımcı metod
    public int GetItemStock(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                return item.stock;
            }
        }
        return 0;
    }

    // Belirli bir ürünün fiyatını almak için yardımcı metod
    public int GetItemPrice(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                return item.price;
            }
        }
        return 0;
    }
    
    void Awake()
    {
        foreach (Item item in items)
        {
            Debug.LogError("asasdfasd");
            item.stock = 0;
        }
    }
}