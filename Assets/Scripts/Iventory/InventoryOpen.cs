using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    public GameObject invetory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            invetory.SetActive(!invetory.activeSelf);
        }
    }
}
