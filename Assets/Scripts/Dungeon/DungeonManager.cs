using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
    public Transform player;
    private DoorPosition lastEnteredDoor;

    public static DungeonManager instance;

    public List<GameObject> Rooms1;
    public List<GameObject> Rooms2;
    public List<GameObject> Rooms3;

    public GameObject currentRoom;

    void Awake()
    {
        if (instance == null) instance = this;
    }

public void EnterRoom(string roomType, DoorPosition enteredDoor)
{
    if (currentRoom != null)
    {
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
        currentRoom = Instantiate(newRoom, Vector3.zero, Quaternion.identity);
    }

    Door[] previousDoors = FindObjectsOfType<Door>();
    foreach (Door door in previousDoors)
    {
        if (door.doorPosition == lastEnteredDoor)
        {
            door.ConvertToExit();
            break;
        }
    }

    PlacePlayerAtNewDoor(enteredDoor);
    lastEnteredDoor = enteredDoor;
}
  void PlacePlayerAtNewDoor(DoorPosition enteredDoor)
    {
        Vector3 newPosition = Vector3.zero;

        switch (enteredDoor)
        {
            case DoorPosition.Top:
                newPosition = GetDoorPosition(DoorPosition.Bottom);
                break;
            case DoorPosition.Bottom:
                newPosition = GetDoorPosition(DoorPosition.Top);
                break;
            case DoorPosition.Left:
                newPosition = GetDoorPosition(DoorPosition.Right);
                break;
            case DoorPosition.Right:
                newPosition = GetDoorPosition(DoorPosition.Left);
                break;
        }

        player.position = newPosition;
    }

    Vector3 GetDoorPosition(DoorPosition doorPos)
    {
        Door[] doors = FindObjectsOfType<Door>();

        foreach (Door door in doors)
        {
            if (door.doorPosition == doorPos)
            {
                return door.transform.position;
            }
        }

        return Vector3.zero;
    }
}
