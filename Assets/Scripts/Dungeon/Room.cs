using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform entrance;
    public Transform[] exits;
    public bool isExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePlayerEnter();
        }
    }

void HandlePlayerEnter()
{
    if (!isExit)
    {
        isExit = true;
        Debug.Log("Player entered room. This room is now an exit.");

        GameManager.Instance.EnterNewRoom();

        GenerateNewRooms();
    }
}

    void GenerateNewRooms()
    {
        foreach (var exit in exits)
        {
            Room newRoom = Instantiate(GameManager.Instance.roomPrefab, exit.position, Quaternion.identity).GetComponent<Room>();

            newRoom.entrance.position = exit.position;
        }
    }
}