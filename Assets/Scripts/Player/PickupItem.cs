using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public int patates;
    public int kisir;
    public int sausage;

    bool kisirTake = false;

    public GameObject alinacakKisir;



    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.CompareTag("Kisir"))
        {
            kisirTake = true;
            alinacakKisir = other.gameObject;
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Kisir"))
        {
            kisirTake = false;
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
                kisir++;
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
