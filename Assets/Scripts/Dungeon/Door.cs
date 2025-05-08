using UnityEngine;

public class Door : MonoBehaviour
{
     public DoorPosition doorPosition;
    public string roomType;
    private bool isLocked = true;

    void Start()
    {
        LockDoor();
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player") && !isLocked)
    {
        Vector3 newRoomPos = DungeonManager.instance.GetNewRoomPositionFromDoor(doorPosition);

        bool roomAlreadyExists = DungeonManager.instance.DoesRoomExistAt(newRoomPos);

        if (PlayerEnergy.instance.currentEnergy > 0 || roomAlreadyExists)
        {
            Debug.Log("Kapı açılıyor: " + doorPosition);
            DungeonManager.instance.EnterRoom(roomType, doorPosition);
        }
    }
}


    public void LockDoor()
    {
        isLocked = true;
    }

    public void ConvertToExit()
{
    roomType = "Exit";
    Debug.Log("Bu kapı artık çıkış kapısı: " + doorPosition);
}

}
