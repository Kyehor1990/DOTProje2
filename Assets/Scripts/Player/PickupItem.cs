using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    bool kisirTake = false;
    bool russalatasiTake = false;

    public GameObject alinacakKisir;
    public GameObject alinacakRussalatasi;

    public Item itemKisir;
    public Item itemRussalatasi;



    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.CompareTag("Kisir"))
        {
            kisirTake = true;
            alinacakKisir = other.gameObject;
        }

        if(other.CompareTag("Russalatasi"))
        {
            russalatasiTake = true;
            alinacakRussalatasi = other.gameObject;
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Kisir"))
        {
            kisirTake = false;
        }

        if(collision.CompareTag("Russalatasi"))
        {
            russalatasiTake = false;
        }
    }

    void Update()
    {
        if (kisirTake)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Kisir picked up");
                TakeItem(0, alinacakKisir);
                itemKisir.stock++;
            }
        }

        if (russalatasiTake)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Russalatasi picked up");
                TakeItem(1, alinacakRussalatasi);
                itemRussalatasi.stock++;
            }
        }
    }

    public void TakeItem(int id ,GameObject other)
    {
       bool result = inventoryManager.AddItem(itemsToPickup[id]);
       if(result == true)
       {
        Debug.Log("Item added to inventory");
        Destroy(other.gameObject);
       }else
       {
        Debug.Log("Inventory is full");
       }
    }
}
