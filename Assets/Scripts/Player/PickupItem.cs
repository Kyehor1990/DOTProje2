using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public int patates;
    public int kısır;
    public int sausage;

    bool kisirTake = false;

    public GameObject alinacak;



    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.CompareTag("Kısır"))
        {
         kisirTake = true;
        }
        alinacak = other.gameObject;

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Kısır"))
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
                Debug.Log("Kısır picked up");
                TakeItem(0, alinacak.GetComponent<Collider2D>());
            }
        }
    }

    public void TakeItem(int id , Collider2D other)
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
