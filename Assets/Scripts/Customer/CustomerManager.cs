using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    public StockManager stockManager;
    public string[] possibleItems; // Patates hariç diğer ürünler
    public float customerInterval = 5f;
    public SceneChange sceneChange;

    private int customerCount;
    private int maxCustomers = 3;

    public Button dungeonButton;
    public GameObject UpgradePanel;
    public GameObject kiraButton;

    // UI elementleri müşteri isteklerini göstermek için
    public GameObject customerOrderUI; // Sipariş gösteren UI paneli
    public Transform orderItemsParent; // Sprite'ların yerleştirileceği parent transform
    public GameObject orderItemPrefab; // Her bir sipariş ürünü için prefab (Image componenti ile)
    public float orderDisplayTime = 3f; // Siparişin kaç saniye gösterileceği

    private List<GameObject> currentOrderItems = new List<GameObject>(); // Şu anki sipariş UI elemanları

    [System.Serializable]
    public class CustomerOrder
    {
        public List<string> requestedItems;
        public bool isOrderComplete;

        public CustomerOrder()
        {
            requestedItems = new List<string>();
            isOrderComplete = false;
        }
    }

    public IEnumerator CustomerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(customerInterval);

            // Müşteri siparişi oluştur
            CustomerOrder order = GenerateCustomerOrder();

            // Siparişi göster
            yield return StartCoroutine(DisplayCustomerOrder(order));

            Debug.Log($"Bir müşteri geldi! İstediği ürünler: {string.Join(", ", order.requestedItems)}");

            // Siparişi işle
            bool orderFulfilled = stockManager.ProcessOrder(order);

            if (orderFulfilled)
            {
                Debug.Log("Müşteri mutlu ve ayrılıyor.");
            }
            else
            {
                Debug.Log("Müşteri üzgün ve ayrılıyor.");
            }

            customerCount++;

            if (customerCount >= maxCustomers)
            {
                Debug.Log("Tüm müşteriler hizmet aldı!");
                UpgradePanel.SetActive(true);
                kiraButton.SetActive(true);

                dungeonButton.gameObject.SetActive(true);
                break;
            }
        }
    }

    private CustomerOrder GenerateCustomerOrder()
    {
        CustomerOrder order = new CustomerOrder();

        // Patates her zaman eklenir - önce stockManager'dan patates ismini alalım
        string potatoName = GetPotatoItemName();
        if (!string.IsNullOrEmpty(potatoName))
        {
            order.requestedItems.Add(potatoName);
        }
        else
        {
            Debug.LogError("Patates ürünü StockManager'da bulunamadı! Item'ların itemName değerlerini kontrol edin.");
        }

        // Rastgele 1-3 arasında ek ürün ekle
        int additionalItemCount = Random.Range(1, 4);

        for (int i = 0; i < additionalItemCount; i++)
        {
            if (possibleItems.Length > 0)
            {
                string randomItem = possibleItems[Random.Range(0, possibleItems.Length)];
                order.requestedItems.Add(randomItem);
            }
        }

        return order;
    }

    private string GetPotatoItemName()
    {
        // StockManager'daki item'lar arasından patates'i bul
        foreach (Item item in stockManager.items)
        {
            // Farklı yazım şekillerini kontrol et
            string itemNameLower = item.itemName.ToLower().Trim();
            if (itemNameLower == "patates" || itemNameLower == "potato" || itemNameLower == "patates kızartması")
            {
                Debug.Log($"Patates bulundu: '{item.itemName}' (Stok: {item.stock})");
                return item.itemName; // Orijinal ismi döndür
            }
        }

        // Eğer bulunamazsa, tüm item'ları listele
        Debug.Log("=== StockManager'daki tüm item'lar ===");
        foreach (Item item in stockManager.items)
        {
            Debug.Log($"Item: '{item.itemName}' - Stok: {item.stock}");
        }

        return null;
    }

    private IEnumerator DisplayCustomerOrder(CustomerOrder order)
    {
        if (customerOrderUI != null && orderItemsParent != null && orderItemPrefab != null)
        {
            // Önceki sipariş UI elemanlarını temizle
            ClearOrderUI();

            // Sipariş UI'sini aktif et
            customerOrderUI.SetActive(true);

            // Her bir sipariş ürünü için sprite oluştur
            foreach (string itemName in order.requestedItems)
            {
                // Item'ın sprite'ını bul
                Sprite itemSprite = GetItemSprite(itemName);

                if (itemSprite != null)
                {
                    // Yeni sipariş item UI elemanı oluştur
                    GameObject orderItemUI = Instantiate(orderItemPrefab, orderItemsParent);

                    // Image componentini bul ve sprite'ı ata
                    UnityEngine.UI.Image imageComponent = orderItemUI.GetComponent<UnityEngine.UI.Image>();
                    if (imageComponent != null)
                    {
                        imageComponent.sprite = itemSprite;
                    }

                    // Listeye ekle (daha sonra temizlemek için)
                    currentOrderItems.Add(orderItemUI);
                }
                else
                {
                    Debug.LogWarning($"Sprite bulunamadı: {itemName}");
                }
            }

            // Belirtilen süre kadar bekle
            yield return new WaitForSeconds(orderDisplayTime);

            // UI'yi kapat ve temizle
            ClearOrderUI();
            customerOrderUI.SetActive(false);
        }
        else
        {
            // UI yoksa sadece console'da göster
            Debug.LogWarning("CustomerOrderUI, OrderItemsParent veya OrderItemPrefab atanmamış!");
            yield return new WaitForSeconds(1f);
        }
    }

    private Sprite GetItemSprite(string itemName)
    {
        // StockManager'daki item listesinden sprite'ı bul
        foreach (Item item in stockManager.items)
        {
            if (item.itemName == itemName)
            {
                return item.image;
            }
        }
        return null;
    }

    private void ClearOrderUI()
    {
        // Mevcut sipariş UI elemanlarını yok et
        foreach (GameObject orderItem in currentOrderItems)
        {
            if (orderItem != null)
            {
                DestroyImmediate(orderItem);
            }
        }
        currentOrderItems.Clear();
    }

    public void ResetCustomerCount()
    {
        customerCount = 0;

        // UI'yi kapat ve temizle (eğer açıksa)
        if (customerOrderUI != null)
        {
            ClearOrderUI();
            customerOrderUI.SetActive(false);
        }
    }

    public void UpgradeCustomer()
    {
        maxCustomers += 1;
    }
}