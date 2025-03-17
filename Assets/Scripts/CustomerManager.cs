using UnityEngine;
using System.Collections;

public class CustomerManager : MonoBehaviour
{
    public StockManager stockManager; // Reference to stock system
    public string[] possibleItems; // List of items customers can request
    public float customerInterval = 5f; // Time between customers

    void Start()
    {
        //StartCoroutine(CustomerRoutine());
    }

    IEnumerator CustomerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(customerInterval);

            string requestedItem = possibleItems[Random.Range(0, possibleItems.Length)];
            Debug.Log($"A customer arrived! They want: {requestedItem}");

            bool sold = stockManager.ProcessOrder(requestedItem);

            if (sold)
            {
                Debug.Log("Customer is happy and leaves.");
            }
            else
            {
                Debug.Log("Customer is sad and leaves.");
            }
        }
    }
}
