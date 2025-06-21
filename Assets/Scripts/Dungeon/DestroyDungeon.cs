using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;

public class DestroyDungeon : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public SceneChange sceneChange;
    public PlayerHealth playerHealth;
    public DungeonManager dungeonManager;

    [Header("Room Creation")]
    public List<GameObject> roomPrefabs;
    public Vector3 spawnPosition = Vector3.zero;

    private readonly string[] tagsToDestroy = { "room", "Kisir", "Patates", "Sosis", "Turşu" };

    public void DungeonDestroy()
    {
        Debug.Log("Dungeon siliniyor...");

        // Tüm dungeon objelerini topla ve sil
        List<GameObject> objectsToDestroy = new List<GameObject>();

        foreach (string tag in tagsToDestroy)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            objectsToDestroy.AddRange(taggedObjects);
        }

        // Objeleri sil
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        // DungeonManager'ı tamamen resetle
        if (dungeonManager != null)
        {
            dungeonManager.ResetDungeon();
        }

        // Oyuncuyu başlangıç pozisyonuna taşı
        if (player != null)
        {
            player.transform.position = spawnPosition;
        }

        Debug.Log($"Toplam {objectsToDestroy.Count} obje silindi ve dungeon resetlendi.");
    }

    public void DungeonCreate()
    {
        if (roomPrefabs == null || roomPrefabs.Count == 0)
        {
            Debug.LogError("Room prefab listesi boş! Inspector'dan prefab ataması yapın.");
            return;
        }

        // Rastgele bir starting room seç
        GameObject selectedRoom = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        GameObject newRoom = Instantiate(selectedRoom, spawnPosition, Quaternion.identity);

        // DungeonManager'da yeni dungeon'u başlat
        if (dungeonManager != null)
        {
            dungeonManager.InitializeNewDungeon(newRoom);
        }

        // NavMesh'i güncelle
        StartCoroutine(UpdateNavMeshAfterFrame());

        // Oyuncuyu başlangıç pozisyonuna taşı
        if (player != null)
        {
            player.transform.position = spawnPosition;
        }

        Debug.Log("Yeni dungeon oluşturuldu ve başlatıldı.");
    }

    private IEnumerator UpdateNavMeshAfterFrame()
    {
        // Bir frame bekle ki objeler tam olarak instantiate olsun
        yield return null;
        
        NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
            Debug.Log("NavMesh güncellendi.");
        }
        else
        {
            Debug.LogWarning("NavMeshSurface bulunamadı. NavMesh oluşturulamadı.");
        }
    }

    // Utility method - gerekirse kullanabilirsiniz
    public void ResetDungeon()
    {
        DungeonDestroy();
        StartCoroutine(CreateDungeonAfterDestroy());
    }

    private IEnumerator CreateDungeonAfterDestroy()
    {
        // Destruction'ın ve DungeonManager reset'inin tamamlanması için bekle
        yield return new WaitForSeconds(0.2f);
        DungeonCreate();
    }

    // Debug için - dungeon durumu kontrolü
    public void LogDungeonStatus()
    {
        if (dungeonManager != null)
        {
            Debug.Log($"Dungeon Status - Room Count: {dungeonManager.GetRoomCount()}, Current Position: {dungeonManager.GetCurrentRoomPosition()}");
        }
    }
}