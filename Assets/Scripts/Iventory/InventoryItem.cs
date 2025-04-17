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
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
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
        transform.SetParent(parentAfterDrag);
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

}
