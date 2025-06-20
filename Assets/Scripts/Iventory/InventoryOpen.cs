using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    public GameObject invetory;

    public SceneChange sceneChange;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && sceneChange.Dungeon)
        {
            invetory.SetActive(!invetory.activeSelf);
        }
    }
}
