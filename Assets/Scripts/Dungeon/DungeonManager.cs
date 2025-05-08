using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;

public class DungeonManager : MonoBehaviour
{
    public Transform player;
    private Dictionary<Vector3, GameObject> generatedRooms = new Dictionary<Vector3, GameObject>();
    public List<GameObject> Rooms1, Rooms2, Rooms3;
    public GameObject startingRoom;

    private Vector3 currentRoomPosition = Vector3.zero;

    public static DungeonManager instance;

    public Camera mainCamera;

    void Awake()
    {
        if (instance == null) instance = this;
        generatedRooms[currentRoomPosition] = startingRoom;
    }

    public void EnterRoom(string roomType, DoorPosition enteredDoor)
    {
        Vector3 newRoomPosition = GetNewRoomPosition(currentRoomPosition, enteredDoor);

        if (!generatedRooms.ContainsKey(newRoomPosition))
        {
            List<GameObject> selectedRoomList = GetRoomList(roomType);
            if (selectedRoomList != null && selectedRoomList.Count > 0 && PlayerEnergy.instance.currentEnergy>0)
            {
                GameObject newRoomPrefab = selectedRoomList[Random.Range(0, selectedRoomList.Count)];
                GameObject newRoom = Instantiate(newRoomPrefab, newRoomPosition, Quaternion.identity);
                generatedRooms[newRoomPosition] = newRoom;
                PlayerEnergy.instance.UseEnergy(1); 
            }

        }

        player.position = GetDoorPosition(newRoomPosition, GetOppositeDoor(enteredDoor));
        currentRoomPosition = newRoomPosition;
    }

    private List<GameObject> GetRoomList(string roomType)
    {
        if (roomType == "1") return Rooms1;
        if (roomType == "2") return Rooms2;
        if (roomType == "3") return Rooms3;
        return null;
    }

    private Vector3 GetNewRoomPosition(Vector3 currentPosition, DoorPosition enteredDoor)
    {
        switch (enteredDoor)
        {
            case DoorPosition.Top: return currentPosition + new Vector3(0, 20, 0);
            case DoorPosition.Bottom: return currentPosition + new Vector3(0, -20, 0);
            case DoorPosition.Left: return currentPosition + new Vector3(-20, 0, 0);
            case DoorPosition.Right: return currentPosition + new Vector3(20, 0, 0);

        }

        return currentPosition;

    }

    private DoorPosition GetOppositeDoor(DoorPosition door)
    {
        switch (door)
        {
            case DoorPosition.Top: return DoorPosition.Bottom;
            case DoorPosition.Bottom: return DoorPosition.Top;
            case DoorPosition.Left: return DoorPosition.Right;
            case DoorPosition.Right: return DoorPosition.Left;
        }
        return door;
    }

private Vector3 GetDoorPosition(Vector3 roomPosition, DoorPosition doorPos)
{

    Debug.Log("GetDoorPosition: " + roomPosition + " " + doorPos);
    mainCamera.transform.position = new Vector3(roomPosition.x +3.21f, roomPosition.y, -10);
    if (generatedRooms.ContainsKey(roomPosition))
    {
        Door[] doors = generatedRooms[roomPosition].GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            if (door.doorPosition == doorPos)
            {
                Vector3 position = door.transform.position;

                switch (doorPos)
                {
                    case DoorPosition.Top:
                        position += new Vector3(0, -1.5f, 0);
                        break;
                    case DoorPosition.Bottom:
                        position += new Vector3(0, 1.5f, 0);
                        break;
                    case DoorPosition.Left:
                        position += new Vector3(1.5f, 0, 0);
                        break;
                    case DoorPosition.Right:
                        position += new Vector3(-1.5f, 0, 0);
                        break;
                }

                return position;
            }
        }
    }
    return roomPosition;
}

}
