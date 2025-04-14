using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Rendering;

public class CustomerManager : MonoBehaviour
{
    public StockManager stockManager;
    public string[] possibleItems;
    public float customerInterval = 5f;

    public SceneChange sceneChange;

    private int customerCount;

    private int maxCustomers =4;

    public IEnumerator CustomerRoutine()
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

            customerCount++;

            if (customerCount >= 4)
            {
                Debug.Log("All customers have been served!");
                break;
            }
        }
    }

    public void ResetCustomerCount()
    {
        customerCount = maxCustomers;
    }
}
