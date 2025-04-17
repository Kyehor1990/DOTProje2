using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite image;

    public string itemName;
    public int stock;
    public int price;

    public int width;
    public int height;

    public GameObject itemPrefab;

}

