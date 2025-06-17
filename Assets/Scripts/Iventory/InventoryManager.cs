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

    void Start()
    {
        gridUsed = new bool[gridWidth, gridHeight];

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
    }

    public bool AddItem(Item item)
    {
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

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (gridUsed[startX + x, startY + y])
                    return false;
            }
        }
        return true;
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
}