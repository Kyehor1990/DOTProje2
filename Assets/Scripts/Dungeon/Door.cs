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
        Debug.Log("Kapı açılıyor: " + doorPosition);
        DungeonManager.instance.EnterRoom(roomType, doorPosition);
        PlayerEnergy.instance.UseEnergy(10); 
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
