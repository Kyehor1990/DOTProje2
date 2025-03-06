using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public List<GameObject> Rooms1;
    public List<GameObject> Rooms2;
    public List<GameObject> Rooms3;

    private GameObject currentRoom;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void EnterRoom(string roomType, Vector3 spawnPosition)
    {
        if (currentRoom != null) Destroy(currentRoom);

        List<GameObject> selectedRoomList = null;
        if (roomType == "1") selectedRoomList = Rooms1;
        else if (roomType == "2") selectedRoomList = Rooms2;
        else if (roomType == "3") selectedRoomList = Rooms3;

        if (selectedRoomList != null && selectedRoomList.Count > 0)
        {
            GameObject newRoom = selectedRoomList[Random.Range(0, selectedRoomList.Count)];
            currentRoom = Instantiate(newRoom, spawnPosition, Quaternion.identity);
        }
    }
}