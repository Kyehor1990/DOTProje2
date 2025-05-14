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



    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
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
    manager.ClearSpace(posX, posY, item.width, item.height);
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

    InventorySlot targetSlot = null;
    
    // Fare altındaki slotu bul
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

    if (targetSlot != null)
    {
        InventoryManager manager = FindObjectOfType<InventoryManager>();
        bool canPlace = manager.CheckSpace(targetSlot.x, targetSlot.y, item.width, item.height);

if (canPlace)
{
    // Grid güncelle
    manager.ClearSpace(posX, posY, item.width, item.height); // eski alanı boşalt
    manager.OccupySpace(targetSlot.x, targetSlot.y, item.width, item.height); // yeni alanı doldur

    // Yeni konumu kaydet
    posX = targetSlot.x;
    posY = targetSlot.y;

    // Eşyayı slotun üzerine yerleştir
    transform.SetParent(targetSlot.transform);
    transform.position = targetSlot.transform.position;
    parentAfterDrag = targetSlot.transform;
    return;
}

    }

    // Uygun değilse geri dön
    Debug.Log("Uygun alan yok");
    transform.SetParent(originalParent);
    transform.position = originalPosition;

    InventoryManager manager1 = FindObjectOfType<InventoryManager>();
    manager1.OccupySpace(posX, posY, item.width, item.height);
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
                Instantiate(itemPrefab,player.transform.position, Quaternion.identity);
                Destroy(gameObject);
                item.stock--;
            }
        }
    }

    public void RotateItem()
    {
        int temp = item.width;
        item.width = item.height;
        item.height = temp;
        // UI'da döndürmek için RectTransform.rotation da ayarlanabilir
    }


}
