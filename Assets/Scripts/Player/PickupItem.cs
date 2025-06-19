using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    bool kisirTake = false;
    bool patatesTake = false;
    bool sosisTake = false;

    public GameObject alinacakKisir;
    public GameObject alınacakPatates;
    public GameObject alinacakSosis;

    public Item itemKisir;
    public Item itemPatates;
    public Item itemSosis;



    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kisir"))
        {
            kisirTake = true;
            alinacakKisir = other.gameObject;
        }

        if (other.CompareTag("Patates"))
        {
            patatesTake = true;
            alınacakPatates = other.gameObject;
        }
        
        if (other.CompareTag("Sosis"))
        {
            sosisTake = true;
            alinacakSosis = other.gameObject;
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Kisir"))
        {
            kisirTake = false;
        }

        if (collision.CompareTag("Patates"))
        {
            patatesTake = false;
        }
        
        if (collision.CompareTag("Sosis"))
        {
            sosisTake = false;
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

        if (patatesTake)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Patates picked up");
                TakeItem(1, alınacakPatates);
                itemPatates.stock++;
            }
        }

        if (sosisTake)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Sosis picked up");
                TakeItem(2, alinacakSosis);
                itemSosis.stock++;
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
