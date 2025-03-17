using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    void OnTriggerStay2D(Collider2D other)
    {        
        if (other.CompareTag("Kısır") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Kısır picked up");
            TakeItem(0, other);
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
