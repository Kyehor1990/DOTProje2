using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    bool isDragging = false;
    GameObject player;
    public Item item;
    GameObject itemPrefab;

    [Header("UI")]
    
    [HideInInspector] public Image image;
    [HideInInspector] public Transform parentAfterDrag;

    [HideInInspector] public Transform originalParent; // sürüklemeden önceki parent
[HideInInspector] public Vector3 originalPosition; // sürüklemeden önceki pozisyon

[HideInInspector] public int posX;
[HideInInspector] public int posY;
[HideInInspector] public int currentWidth;
    [HideInInspector] public int currentHeight;

private int dragStartWidth;
private int dragStartHeight;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


public void InitialiseItem(Item newItem)
{
    item = newItem;
    image.sprite = newItem.image;

    currentWidth = item.width;
    currentHeight = item.height;
}


public void OnBeginDrag(PointerEventData eventData)
{
    isDragging = true;
    image.raycastTarget = false;

    originalParent = transform.parent;
    originalPosition = transform.position;
    parentAfterDrag = transform.parent;
    transform.SetParent(transform.root);

    InventoryManager manager = FindObjectOfType<InventoryManager>();

    // ✅ BOYUTU HAFIZAYA AL
    dragStartWidth = currentWidth;
    dragStartHeight = currentHeight;
}




    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        transform.position = Input.mousePosition;
    }

public void OnEndDrag(PointerEventData eventData)
{
    isDragging = false;
    image.raycastTarget = true;

    InventoryManager manager = FindObjectOfType<InventoryManager>();

    // 1️⃣ ESKİ KONUMU TEMİZLE (her durumda)
    manager.ClearSpace(posX, posY, currentWidth, currentHeight);

    // 2️⃣ Fare altındaki slotu bul
    InventorySlot targetSlot = null;
    PointerEventData pointerData = new PointerEventData(EventSystem.current)
    {
        position = Input.mousePosition
    };

    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerData, results);

    foreach (var result in results)
    {
        targetSlot = result.gameObject.GetComponent<InventorySlot>();
        if (targetSlot != null)
            break;
    }

    // 3️⃣ Yeni pozisyon uygunsa yerleştir
    if (targetSlot != null && manager.CheckSpace(targetSlot.x, targetSlot.y, currentWidth, currentHeight))
    {
        posX = targetSlot.x;
        posY = targetSlot.y;

        transform.SetParent(targetSlot.transform);
        transform.position = targetSlot.transform.position;

        manager.OccupySpace(posX, posY, currentWidth, currentHeight);
    }
    else
    {
        // 4️⃣ Uygun değilse eski yerine geri dön ve eski konumu yeniden işaretle
        transform.SetParent(originalParent);
        transform.position = originalPosition;

        manager.OccupySpace(posX, posY, currentWidth, currentHeight);
    }
}




    void Update()
{
    if (isDragging)
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(item.itemName == "kisir"){
            itemPrefab = item.itemPrefab;
            }
            Instantiate(itemPrefab, player.transform.position, Quaternion.identity);
            Destroy(gameObject);
            item.stock--;
        } 

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }
    }
}

public void RotateItem()
{
    InventoryManager manager = FindObjectOfType<InventoryManager>();

    // Dönmeden önce eski alanı temizle
    manager.ClearSpace(posX, posY, currentWidth, currentHeight);

    // Boyutları değiştir
    int temp = currentWidth;
    currentWidth = currentHeight;
    currentHeight = temp;

    // UI'da döndür
    RectTransform rect = GetComponent<RectTransform>();
    rect.Rotate(0, 0, 90);

    // Yeni boyutla eski pozisyon sığabiliyor mu?
    if (manager.CheckSpace(posX, posY, currentWidth, currentHeight))
    {
        manager.OccupySpace(posX, posY, currentWidth, currentHeight); // Yeni boyutu işaretle
    }
    else
    {
        // Eğer sığmıyorsa geri döndür
        currentHeight = temp;
        currentWidth = currentHeight;
        rect.Rotate(0, 0, -90); // Geri çevir

        manager.OccupySpace(posX, posY, currentWidth, currentHeight); // Eski haliyle geri yerleştir

        Debug.Log("Dönüş başarısız: bu pozisyona yeni boyut sığmıyor.");
    }
}






}
