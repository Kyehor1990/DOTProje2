using UnityEngine;

public class Door : MonoBehaviour
{
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
    if (other.CompareTag("Player")){
        Debug.Log("Kapı kilitli mi? " + isLocked);
    }

    if (other.CompareTag("Player") && !isLocked)
    {
        Debug.Log("Kapı açılıyor...");
        DungeonManager.instance.EnterRoom(roomType, Vector3.zero);
        PlayerEnergy.instance.UseEnergy(10); 
    }
    }

    public void LockDoor()
    {
        isLocked = true;
    }

}
