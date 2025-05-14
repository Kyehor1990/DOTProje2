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
for (int y = gridHeight - 1; y >= 0; y--) // YUKARIDAN AŞAĞI
{
    for (int x = 0; x < gridWidth; x++)
    {
        if (index < inventorySlots.Length)
        {
            inventorySlots[index].x = x;
            inventorySlots[index].y = gridHeight - 1 - y; // Y eksenini tersle
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
    return false; // Hiç uygun yer yoksa
}


    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
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

    // Slotları bul
    List<InventorySlot> usedSlots = new List<InventorySlot>();
    foreach (var slot in inventorySlots)
    {
        if (slot.x >= startX && slot.x < startX + item.width &&
            slot.y >= startY && slot.y < startY + item.height)
        {
            usedSlots.Add(slot);
        }
    }

    // İlk slota prefab oluştur
    GameObject newItemGo = Instantiate(inventoryItemPrefab, usedSlots[0].transform);
    InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
    inventoryItem.InitialiseItem(item);

    // Eşyanın pozisyon bilgisini kaydet
    inventoryItem.posX = startX;
    inventoryItem.posY = startY;
}

public void ClearSpace(int startX, int startY, int width, int height)
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            gridUsed[startX + x, startY + y] = false;
        }
    }
}

public void OccupySpace(int startX, int startY, int width, int height)
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            gridUsed[startX + x, startY + y] = true;
        }
    }
}

   
}

