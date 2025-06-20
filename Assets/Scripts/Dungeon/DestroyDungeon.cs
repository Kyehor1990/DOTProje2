using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;

public class DestroyDungeon : MonoBehaviour
{

    public GameObject player;
    private string tagRoom = "room";

    public List<GameObject> roomPrefabs;
    public Vector3 spawnPosition;

    public SceneChange sceneChange;

    public PlayerHealth playerHealth;

    public void DungeonDestroy()
    {
Debug.Log("Dungeon Silindi.");
GameObject[] items = GameObject.FindGameObjectsWithTag(tagRoom);
GameObject[] items2 = GameObject.FindGameObjectsWithTag("Kisir");
GameObject[] items3 = GameObject.FindGameObjectsWithTag("Patates");
GameObject[] items4 = GameObject.FindGameObjectsWithTag("Sosis");
GameObject[] items5 = GameObject.FindGameObjectsWithTag("Turşu");

// Doğru boyutta array oluştur (tüm itemlar için)
GameObject[] combinedItems = new GameObject[items2.Length + items3.Length + items4.Length + items5.Length];

// Doğru indexlerle kopyala
items2.CopyTo(combinedItems, 0);
items3.CopyTo(combinedItems, items2.Length);
items4.CopyTo(combinedItems, items2.Length + items3.Length);
items5.CopyTo(combinedItems, items2.Length + items3.Length + items4.Length);

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
    if (roomPrefabs == null || roomPrefabs.Count == 0)
    {
        Debug.LogError("Room prefab listesi boş! Inspector'dan atama yapmayı unutma.");
        return;
    }

    GameObject selectedRoom = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
    Instantiate(selectedRoom, spawnPosition, Quaternion.identity);

    NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
    if (surface != null)
    {
        surface.BuildNavMesh();
    }
    else
    {
        Debug.LogWarning("NavMeshSurface bulunamadı. NavMesh oluşturulamadı.");
    }

    player.transform.position = new Vector3(0, 0, 0);
}


}
