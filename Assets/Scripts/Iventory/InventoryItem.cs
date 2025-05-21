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
    image.raycastTarget = false;

    originalParent = transform.parent;
    originalPosition = transform.position;
    parentAfterDrag = transform.parent;
    transform.SetParent(transform.root);

    // Grid boşalt (drag başladığında)
    InventoryManager manager = FindObjectOfType<InventoryManager>();
    // eski alanı boşalt
    manager.ClearSpace(posX, posY, currentWidth, currentHeight);
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

    // Fare altındaki slotu bul
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

    if (targetSlot != null && manager != null)
    {
        // Yeni konumda yer var mı kontrol et
        bool canPlace = manager.CheckSpace(targetSlot.x, targetSlot.y, currentWidth, currentHeight);

        if (canPlace)
        {
            // Yeni yer uygun → grid güncelle
            manager.OccupySpace(targetSlot.x, targetSlot.y, currentWidth, currentHeight);

            // Item’in yeni pozisyonunu kaydet
            posX = targetSlot.x;
            posY = targetSlot.y;

            // UI’da yerleştir
            transform.SetParent(targetSlot.transform);
            transform.position = targetSlot.transform.position;
            parentAfterDrag = targetSlot.transform;
            return;
        }
    }

    // Yeni yer uygun değil → eski yere geri dön ve grid'i geri yaz
    manager.OccupySpace(posX, posY, currentWidth, currentHeight);
    transform.SetParent(originalParent);
    transform.position = originalPosition;
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

    int oldWidth = currentWidth;
    int oldHeight = currentHeight;

    int newWidth = oldHeight;
    int newHeight = oldWidth;

    manager.ClearSpace(posX, posY, currentWidth, currentHeight);


    if (manager.CheckSpace(posX, posY, newWidth, newHeight))
    {
        currentWidth = newWidth;
        currentHeight = newHeight;

        RectTransform rect = GetComponent<RectTransform>();
        rect.Rotate(0, 0, 90);

        manager.OccupySpace(posX, posY, currentWidth, currentHeight);
    }
    else
    {
        manager.OccupySpace(posX, posY, oldWidth, oldHeight);
        Debug.Log("Dönüş başarısız: yeni konuma sığmıyor.");
    }
}





}
