using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private string tagItem = "item";

    public SceneChange sceneChange;

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private int gridWidth = 7;
    private int gridHeight = 4;
    private bool[,] gridUsed;
    
    // Slot satın alma sistemi için yeni değişkenler
    [Header("Slot Upgrade System")]
    public int initialActiveSlots = 14; // Başlangıçta aktif olacak slot sayısı (örn: 2x7=14)
    public int slotUpgradePrice = 75; // Her slot için fiyat
    private bool[] slotsUnlocked; // Hangi slotların açık olduğunu takip eder

    void Start()
    {
        gridUsed = new bool[gridWidth, gridHeight];
        slotsUnlocked = new bool[inventorySlots.Length];

        // Slotları grid pozisyonlarına göre ayarla
        int index = 0;
        for (int y = gridHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (index < inventorySlots.Length)
                {
                    inventorySlots[index].x = x;
                    inventorySlots[index].y = gridHeight - 1 - y;
                    index++;
                }
            }
        }
        
        // Başlangıçta belirli sayıda slot açık
        InitializeSlots();
        UpdateSlotVisibility();
    }

    void InitializeSlots()
    {
        // İlk slotları soldan sağa, yukarıdan aşağıya açar
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < initialActiveSlots)
            {
                slotsUnlocked[i] = true;
            }
            else
            {
                slotsUnlocked[i] = false;
            }
        }
    }

    void UpdateSlotVisibility()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].gameObject.SetActive(slotsUnlocked[i]);
        }
        
        int unlockedCount = 0;
        for (int i = 0; i < slotsUnlocked.Length; i++)
        {
            if (slotsUnlocked[i]) unlockedCount++;
        }
        
        Debug.Log($"Active slots: {unlockedCount}/{inventorySlots.Length}");
    }

    public bool CanUpgradeSlots()
    {
        // Tüm slotlar açık mı kontrol et
        for (int i = 0; i < slotsUnlocked.Length; i++)
        {
            if (!slotsUnlocked[i]) return true;
        }
        return false;
    }
    
    public int GetSlotUpgradePrice()
    {
        return slotUpgradePrice;
    }
    
    public int GetNextSlotToUnlock()
    {
        // Bir sonraki açılacak slotun indeksini bul
        for (int i = 0; i < slotsUnlocked.Length; i++)
        {
            if (!slotsUnlocked[i]) return i;
        }
        return -1; // Tüm slotlar açık
    }
    
    public bool UpgradeSlot(int playerMoney)
    {
        if (!CanUpgradeSlots()) 
        {
            Debug.Log("All slots are already unlocked!");
            return false;
        }
        
        if (playerMoney < slotUpgradePrice)
        {
            Debug.Log($"Not enough money! Need {slotUpgradePrice}, have {playerMoney}");
            return false;
        }
        
        int nextSlotIndex = GetNextSlotToUnlock();
        if (nextSlotIndex == -1) return false;
        
        slotsUnlocked[nextSlotIndex] = true;
        UpdateSlotVisibility();
        
        Debug.Log($"Slot {nextSlotIndex} unlocked!");
        return true;
    }

    // Birden fazla slot satın alma fonksiyonu
    public bool UpgradeMultipleSlots(int playerMoney, int slotCount)
    {
        int totalCost = slotUpgradePrice * slotCount;
        
        if (playerMoney < totalCost)
        {
            Debug.Log($"Not enough money! Need {totalCost}, have {playerMoney}");
            return false;
        }
        
        // Satın alınabilecek slot sayısını kontrol et
        int availableSlots = 0;
        for (int i = 0; i < slotsUnlocked.Length; i++)
        {
            if (!slotsUnlocked[i]) availableSlots++;
        }
        
        if (availableSlots < slotCount)
        {
            Debug.Log($"Only {availableSlots} slots available to unlock!");
            return false;
        }
        
        // Slotları aç
        int unlockedCount = 0;
        for (int i = 0; i < slotsUnlocked.Length && unlockedCount < slotCount; i++)
        {
            if (!slotsUnlocked[i])
            {
                slotsUnlocked[i] = true;
                unlockedCount++;
            }
        }
        
        UpdateSlotVisibility();
        Debug.Log($"{slotCount} slots unlocked for {totalCost} coins!");
        return true;
    }

    public bool AddItem(Item item)
    {
        // Sadece açık slotlarda yer ara
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (CheckSpace(x, y, item.width, item.height))
                {
                    PlaceItem(item, x, y);
                    return true;
                }
            }
        }
        return false;
    }

    void Update()
    {
       if(sceneChange.Dungeon == false){
        GameObject[] items = GameObject.FindGameObjectsWithTag(tagItem);

        foreach (GameObject obj in items)
        {
            Destroy(obj);
        }
       }
    }

    public bool CheckSpace(int startX, int startY, int width, int height)
    {
        if (startX + width > gridWidth || startY + height > gridHeight)
            return false;

        // İlgili slotların açık olup olmadığını kontrol et
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int slotIndex = GetSlotIndex(startX + x, startY + y);
                if (slotIndex == -1 || !slotsUnlocked[slotIndex])
                    return false;
                    
                if (gridUsed[startX + x, startY + y])
                    return false;
            }
        }
        return true;
    }
    
    int GetSlotIndex(int x, int y)
    {
        // Grid pozisyonundan slot indeksini bul
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].x == x && inventorySlots[i].y == y)
            {
                return i;
            }
        }
        return -1;
    }

    void PlaceItem(Item item, int startX, int startY)
    {
        // Grid işaretle
        OccupySpace(startX, startY, item.width, item.height);

        // Sol üst slotu bul (başlangıç pozisyonu)
        InventorySlot targetSlot = null;
        foreach (var slot in inventorySlots)
        {
            if (slot.x == startX && slot.y == startY)
            {
                targetSlot = slot;
                break;
            }
        }

        if (targetSlot != null)
        {
            // Item prefab'ını oluştur
            GameObject newItemGo = Instantiate(inventoryItemPrefab, targetSlot.transform);
            InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
            
            // KRITIK: Pozisyon değerlerini initialize etmeden önce ata
            inventoryItem.posX = startX;
            inventoryItem.posY = startY;
            inventoryItem.currentWidth = item.width;
            inventoryItem.currentHeight = item.height;
            
            // Item'ı initialize et (bu UpdateItemSize'ı çağıracak)
            inventoryItem.InitialiseItem(item);
            
            Debug.Log($"Item placed at ({startX}, {startY}) with size {item.width}x{item.height}");
        }
    }

    public void ClearSpace(int startX, int startY, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (startX + x >= 0 && startX + x < gridWidth && 
                    startY + y >= 0 && startY + y < gridHeight)
                {
                    gridUsed[startX + x, startY + y] = false;
                }
            }
        }
        Debug.Log($"Cleared space at ({startX}, {startY}) with size {width}x{height}");
    }

    public void OccupySpace(int startX, int startY, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (startX + x >= 0 && startX + x < gridWidth && 
                    startY + y >= 0 && startY + y < gridHeight)
                {
                    gridUsed[startX + x, startY + y] = true;
                }
            }
        }
        Debug.Log($"Occupied space at ({startX}, {startY}) with size {width}x{height}");
    }
    
    // Debug için: açık slot sayısını döndür
    public int GetUnlockedSlotCount()
    {
        int count = 0;
        for (int i = 0; i < slotsUnlocked.Length; i++)
        {
            if (slotsUnlocked[i]) count++;
        }
        return count;
    }
    
    // Debug için: toplam slot sayısını döndür
    public int GetTotalSlotCount()
    {
        return inventorySlots.Length;
    }
}