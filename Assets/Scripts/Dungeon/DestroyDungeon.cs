using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDungeon : MonoBehaviour
{

    public GameObject player;
    private string tagRoom = "room";

    public GameObject prefabRoom;
    public Vector3 spawnPosition;

    public SceneChange sceneChange;

    public PlayerHealth playerHealth;

    public void DungeonDestroy()
    {
        Debug.Log("Dungeon Silindi.");
        GameObject[] items = GameObject.FindGameObjectsWithTag(tagRoom);
        GameObject[] items2 = GameObject.FindGameObjectsWithTag("Kisir");
        GameObject[] items3 = GameObject.FindGameObjectsWithTag("Russalatasi");
        GameObject[] combinedItems = new GameObject[items2.Length + items3.Length];
        items2.CopyTo(combinedItems, 0);
        items3.CopyTo(combinedItems, items2.Length);

        foreach (GameObject obj in combinedItems)
        {
            Destroy(obj);
        }

            foreach (GameObject obj in items)
        {
            Destroy(obj);
        }
    }

    public void DungeonCreate()
    {
         Instantiate(prefabRoom, spawnPosition, Quaternion.identity);

         player.transform.position = new Vector3(0, 0, 0);
       
    }




}
