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

    [HideInInspector] public Transform originalParent;
    [HideInInspector] public Vector3 originalPosition;

    [HideInInspector] public int posX;
    [HideInInspector] public int posY;
    [HideInInspector] public int currentWidth;
    [HideInInspector] public int currentHeight;

    private int dragStartWidth;
    private int dragStartHeight;

    // Slot boyutunu hesaplamak için referans
    private float slotSize = 50f;
    
    // Döndürme durumunu takip et
    private bool isRotated = false; // Basit true/false kontrolü

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        
        // Canvas Group ekle ve sorting order ayarla
        Canvas itemCanvas = gameObject.GetComponent<Canvas>();
        if (itemCanvas == null)
        {
            itemCanvas = gameObject.AddComponent<Canvas>();
        }
        itemCanvas.overrideSorting = true;
        itemCanvas.sortingOrder = 100;
        
        // GraphicRaycaster ekle
        if (gameObject.GetComponent<GraphicRaycaster>() == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }
        
        // Slot boyutunu otomatik hesapla
        InventoryManager manager = FindObjectOfType<InventoryManager>();
        if (manager != null && manager.inventorySlots.Length > 0)
        {
            RectTransform slotRect = manager.inventorySlots[0].GetComponent<RectTransform>();
            slotSize = slotRect.sizeDelta.x;
        }
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;

        currentWidth = item.width;
        currentHeight = item.height;
        
        // Item boyutunu ayarla
        UpdateItemSize();
    }

    void UpdateItemSize()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        
        // Boyutu ayarla
        rectTransform.sizeDelta = new Vector2(currentWidth * slotSize, currentHeight * slotSize);
        
        // Orijinal kodunuzdaki pivot ve anchor ayarlarını koruyorum
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        
        // Orijinal kodunuzdaki offset hesaplamasını koruyorum
        Vector2 offset = new Vector2(
            (currentWidth - 1) * slotSize * 0.5f,
            -(currentHeight - 1) * slotSize * 0.5f
        );
        rectTransform.anchoredPosition = offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        image.raycastTarget = false;

        originalParent = transform.parent;
        originalPosition = transform.position;
        parentAfterDrag = transform.parent;
        
        Debug.Log($"OnBeginDrag: Item at position ({posX}, {posY}) with size {currentWidth}x{currentHeight}");
        
        transform.SetParent(transform.root);

        // Canvas sorting order'ı sürüklerken daha da yükselt
        Canvas itemCanvas = GetComponent<Canvas>();
        if (itemCanvas != null)
        {
            itemCanvas.sortingOrder = 200;
        }

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

        // Eski konumu temizle
        manager.ClearSpace(posX, posY, currentWidth, currentHeight);

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

        // Canvas sorting order'ı normale döndür
        Canvas itemCanvas = GetComponent<Canvas>();
        if (itemCanvas != null)
        {
            itemCanvas.sortingOrder = 100;
        }

        // Yeni pozisyon uygunsa yerleştir
        if (targetSlot != null && manager.CheckSpace(targetSlot.x, targetSlot.y, currentWidth, currentHeight))
        {
            posX = targetSlot.x;
            posY = targetSlot.y;

            // Parent'ı hedef slota ayarla
            transform.SetParent(targetSlot.transform);
            
            // Boyutu ve pozisyonu yeniden ayarla
            UpdateItemSize();

            manager.OccupySpace(posX, posY, currentWidth, currentHeight);
        }
        else
        {
            // Uygun değilse eski yerine geri dön
            transform.SetParent(originalParent);
            transform.position = originalPosition;
            
            // Boyutu ve pozisyonu yeniden ayarla
            UpdateItemSize();

            manager.OccupySpace(posX, posY, currentWidth, currentHeight);
        }
    }

    void Update()
    {
        if (isDragging)
        {
            PickupItem pickupItem = player.GetComponent<PickupItem>();
            InventoryManager manager = FindObjectOfType<InventoryManager>();

            if (Input.GetKeyDown(KeyCode.Q))
            {

                if (item.itemName == "kisir")
                {
                    itemPrefab = item.itemPrefab;
                    pickupItem.kisirStock--;

                }
                else if (item.itemName == "Patates")
                {
                    itemPrefab = item.itemPrefab;
                    pickupItem.patatesStock--;

                }
                else if (item.itemName == "Sosis")
                {
                    itemPrefab = item.itemPrefab;
                    pickupItem.sosisStock--;
                }
                else if (item.itemName == "Turşu")
                {
                    itemPrefab = item.itemPrefab;
                    pickupItem.turşuStock--;
                }

                manager.ClearSpace(posX, posY, currentWidth, currentHeight);
                Instantiate(itemPrefab, player.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    RotateItem();
                }
            
        }
    }

    public void RotateItem()
    {
        // 1x1 itemlar için döndürme işlemi yapma
        if (item.width == 1 && item.height == 1)
        {
            return;
        }

        InventoryManager manager = FindObjectOfType<InventoryManager>();

        // Dönmeden önce eski alanı temizle
        manager.ClearSpace(posX, posY, currentWidth, currentHeight);

        // Boyutları değiştir
        int temp = currentWidth;
        currentWidth = currentHeight;
        currentHeight = temp;

        // Rotation durumunu tersine çevir
        isRotated = !isRotated;

        // Yeni boyutla eski pozisyon sığabiliyor mu?
        if (manager.CheckSpace(posX, posY, currentWidth, currentHeight))
        {
            // RectTransform referansı al
            RectTransform rect = GetComponent<RectTransform>();
            
            // Boyutu güncelle
            UpdateItemSize();
            
            // Rotasyonu uygula
            rect.localRotation = isRotated ? Quaternion.Euler(0, 0, 90f) : Quaternion.Euler(0, 0, 0f);
            
            manager.OccupySpace(posX, posY, currentWidth, currentHeight);
            
            Debug.Log($"Item rotated. New size: {currentWidth}x{currentHeight}, Rotated: {isRotated}");
        }
        else
        {
            // Eğer sığmıyorsa boyutları ve rotation durumunu geri al
            temp = currentWidth;
            currentWidth = currentHeight;
            currentHeight = temp;
            
            isRotated = !isRotated;

            manager.OccupySpace(posX, posY, currentWidth, currentHeight);

            Debug.Log("Rotation failed: new size doesn't fit in current position.");
        }
    }
}