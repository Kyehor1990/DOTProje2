using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    public GameObject invetory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            invetory.SetActive(!invetory.activeSelf);
        }
    }
}
