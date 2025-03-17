using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public TileBase tile;
    public Sprite image;
    public ItemType type;
    public Vector2Int range = new Vector2Int(5, 4);

    public int width;
    public int height;

}

public enum ItemType
{
    Sosis,
    Kısır,
    RusSalatası,
}

