using UnityEngine;
using System.Collections;

public class CustomerManager : MonoBehaviour
{
    public StockManager stockManager;
    public string[] possibleItems;
    public float customerInterval = 5f;

    public CameraControl cameraControl;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C) && cameraControl.Dungeon == false)
        {
            StartCoroutine(CustomerRoutine());
        } else if (Input.GetKeyDown(KeyCode.C) && cameraControl.Dungeon == true)
        {
            StopAllCoroutines();
        }
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
