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
    private bool isRotated = false;

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
        
        // RectTransform boyutunu her zaman orijinal item boyutuna göre ayarla
        // Döndürme durumunda bile RectTransform boyutu değişmez, sadece rotation uygulanır
        rectTransform.sizeDelta = new Vector2(item.width * slotSize, item.height * slotSize);
        
        // Pivot ve anchor ayarlarını koru
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        
        // Offset hesaplamasını grid pozisyonuna göre ayarla (döndürme durumu değil)
        // currentWidth ve currentHeight grid'deki yer kaplama durumunu gösterir
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

        // Geçici değişkenler ile yeni boyutları hesapla (sadece grid için)
        int newWidth = currentHeight;
        int newHeight = currentWidth;

        // Yeni boyutla eski pozisyon sığabiliyor mu kontrol et
        if (manager.CheckSpace(posX, posY, newWidth, newHeight))
        {
            // Grid boyutlarını güncelle (sadece yer kaplama için)
            currentWidth = newWidth;
            currentHeight = newHeight;
            
            // Rotation durumunu tersine çevir
            isRotated = !isRotated;

            // RectTransform referansı al
            RectTransform rect = GetComponent<RectTransform>();
            
            // Rotasyonu uygula (RectTransform boyutu değişmez)
            rect.localRotation = isRotated ? Quaternion.Euler(0, 0, 90f) : Quaternion.identity;
            
            // Pozisyonu güncelle (boyut sabit kalır)
            UpdateItemSize();
            
            // Yeni alanı işgal et
            manager.OccupySpace(posX, posY, currentWidth, currentHeight);
            
            Debug.Log($"Item rotated. Grid size: {currentWidth}x{currentHeight}, Visual rotated: {isRotated}");
        }
        else
        {
            // Eğer sığmıyorsa eski alanı geri işgal et
            manager.OccupySpace(posX, posY, currentWidth, currentHeight);

            Debug.Log("Rotation failed: new size doesn't fit in current position.");
        }
    }
}