using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public List<GameObject> Rooms1;
    public List<GameObject> Rooms2;
    public List<GameObject> Rooms3;

    public GameObject currentRoom;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

public void EnterRoom(string roomType, Vector3 spawnPosition)
{
    if (currentRoom != null)
    {
        Debug.Log("Önceki oda yok ediliyor.");
        Destroy(currentRoom);
    }

    List<GameObject> selectedRoomList = null;

    if (roomType == "1") selectedRoomList = Rooms1;
    else if (roomType == "2") selectedRoomList = Rooms2;
    else if (roomType == "3") selectedRoomList = Rooms3;

    if (selectedRoomList != null && selectedRoomList.Count > 0)
    {
        GameObject newRoom = selectedRoomList[Random.Range(0, selectedRoomList.Count)];
        Debug.Log("Yeni oda oluşturuluyor: " + newRoom.name); 
        currentRoom = Instantiate(newRoom, spawnPosition, Quaternion.identity);
    }
    else
    {
        Debug.LogError("Hata! Oda listesi boş veya oda bulunamadı.");
    }
}
}