
using UnityEngine;
using System.Collections.Generic;
using NavMeshPlus.Components;

public class DungeonManager : MonoBehaviour
{
    [Header("Player ve Camera")]
    public Transform player;
    public Camera mainCamera;

    [Header("Room Prefabs")]
    public List<GameObject> Rooms1, Rooms2, Rooms3, Rooms4, Rooms5;
    public GameObject startingRoom;

    private Dictionary<Vector3, GameObject> generatedRooms = new Dictionary<Vector3, GameObject>();
    private Vector3 currentRoomPosition = Vector3.zero;

    public static DungeonManager instance;

    void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        // Starting room'u dictionary'e ekle
        if (startingRoom != null)
        {
            generatedRooms[currentRoomPosition] = startingRoom;
        }
    }

    public void EnterRoom(string roomType, DoorPosition enteredDoor)
    {
        Vector3 newRoomPosition = GetNewRoomPosition(currentRoomPosition, enteredDoor);
        
        Debug.Log($"Yeni odaya giriliyor: {newRoomPosition}");

        // Eğer bu pozisyonda oda yoksa yeni oda oluştur
        if (!generatedRooms.ContainsKey(newRoomPosition))
        {
            List<GameObject> selectedRoomList = GetRoomList(roomType);
            
            if (selectedRoomList != null && selectedRoomList.Count > 0)
            {
                // Energy kontrolü - sadece yeni oda oluştururken
                if (PlayerEnergy.instance != null && PlayerEnergy.instance.currentEnergy > 0)
                {
                    GameObject newRoomPrefab = selectedRoomList[Random.Range(0, selectedRoomList.Count)];
                    GameObject newRoom = Instantiate(newRoomPrefab, newRoomPosition, Quaternion.identity);
                    generatedRooms[newRoomPosition] = newRoom;
                    
                    PlayerEnergy.instance.UseEnergy(1);
                    
                    // NavMesh'i güncelle
                    UpdateNavMesh();
                    
                    Debug.Log($"Yeni oda oluşturuldu: {newRoomPosition}");
                }
                else
                {
                    Debug.Log("Yeterli enerji yok, yeni oda oluşturulamadı!");
                    return;
                }
            }
            else
            {
                Debug.LogError($"Room type {roomType} için prefab bulunamadı!");
                return;
            }
        }

        // Oyuncuyu yeni pozisyona taşı
        Vector3 playerSpawnPos = GetDoorPosition(newRoomPosition, GetOppositeDoor(enteredDoor));
        if (player != null)
        {
            player.position = playerSpawnPos;
        }

        // Kamera pozisyonunu güncelle
        UpdateCameraPosition(newRoomPosition);
        
        currentRoomPosition = newRoomPosition;
    }

    private void UpdateNavMesh()
    {
        NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
    }

    private void UpdateCameraPosition(Vector3 roomPosition)
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(roomPosition.x + 3.21f, roomPosition.y - 0.58f, -10);
        }
    }

    private List<GameObject> GetRoomList(string roomType)
    {
        switch (roomType)
        {
            case "1": return Rooms1;
            case "2": return Rooms2;
            case "3": return Rooms3;
            case "4": return Rooms4;
            case "5": return Rooms5;
            default: 
                Debug.LogWarning($"Bilinmeyen room type: {roomType}");
                return null;
        }
    }

    private Vector3 GetNewRoomPosition(Vector3 currentPosition, DoorPosition enteredDoor)
    {
        switch (enteredDoor)
        {
            case DoorPosition.Top: return currentPosition + new Vector3(0, 20, 0);
            case DoorPosition.Bottom: return currentPosition + new Vector3(0, -20, 0);
            case DoorPosition.Left: return currentPosition + new Vector3(-30, 0, 0);
            case DoorPosition.Right: return currentPosition + new Vector3(30, 0, 0);
            default: return currentPosition;
        }
    }

    private DoorPosition GetOppositeDoor(DoorPosition door)
    {
        switch (door)
        {
            case DoorPosition.Top: return DoorPosition.Bottom;
            case DoorPosition.Bottom: return DoorPosition.Top;
            case DoorPosition.Left: return DoorPosition.Right;
            case DoorPosition.Right: return DoorPosition.Left;
            default: return door;
        }
    }

    private Vector3 GetDoorPosition(Vector3 roomPosition, DoorPosition doorPos)
    {
        if (generatedRooms.ContainsKey(roomPosition))
        {
            Door[] doors = generatedRooms[roomPosition].GetComponentsInChildren<Door>();
            foreach (Door door in doors)
            {
                if (door.doorPosition == doorPos)
                {
                    Vector3 position = door.transform.position;

                    // Oyuncuyu kapının içine değil, yanına spawn et
                    switch (doorPos)
                    {
                        case DoorPosition.Top:
                            position += new Vector3(0, -2f, 0);
                            break;
                        case DoorPosition.Bottom:
                            position += new Vector3(0, 2f, 0);
                            break;
                        case DoorPosition.Left:
                            position += new Vector3(2f, 0, 0);
                            break;
                        case DoorPosition.Right:
                            position += new Vector3(-2f, 0, 0);
                            break;
                    }

                    return position;
                }
            }
        }
        
        // Eğer kapı bulunamazsa odanın merkezini döndür
        return roomPosition;
    }

    public Vector3 GetNewRoomPositionFromDoor(DoorPosition enteredDoor)
    {
        return GetNewRoomPosition(currentRoomPosition, enteredDoor);
    }

    public bool DoesRoomExistAt(Vector3 position)
    {
        return generatedRooms.ContainsKey(position);
    }

    public void CleanGeneratedRooms()
    {
        List<Vector3> keysToRemove = new List<Vector3>();
        foreach (var pair in generatedRooms)
        {
            if (pair.Value == null)
            {
                keysToRemove.Add(pair.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            generatedRooms.Remove(key);
            Debug.LogWarning($"Null oda silindi: {key}");
        }
    }

    // Tüm dungeon'u resetle - destroy işleminde kullanılacak
    public void ResetDungeon()
    {
        Debug.Log("DungeonManager resetleniyor...");
        
        // Tüm generated rooms'u temizle
        generatedRooms.Clear();
        
        // Pozisyonu sıfırla
        currentRoomPosition = Vector3.zero;
        
        // Starting room referansını sıfırla
        startingRoom = null;
        
        Debug.Log("DungeonManager resetlendi.");
    }

    // Yeni dungeon başlatma
    public void InitializeNewDungeon(GameObject newStartingRoom)
    {
        Debug.Log("Yeni dungeon başlatılıyor...");
        
        // Önce her şeyi temizle
        ResetDungeon();
        
        // Yeni starting room'u set et
        startingRoom = newStartingRoom;
        currentRoomPosition = Vector3.zero;
        
        // Starting room'u dictionary'e ekle
        if (startingRoom != null)
        {
            generatedRooms[currentRoomPosition] = startingRoom;
            Debug.Log("Yeni starting room eklendi.");
        }
        
        // Kamera pozisyonunu güncelle
        UpdateCameraPosition(currentRoomPosition);
    }

    public Vector3 GetCurrentRoomPosition()
    {
        return currentRoomPosition;
    }

    public void SetCurrentRoomPosition(Vector3 position)
    {
        currentRoomPosition = position;
    }

    // Debug için - kaç oda var
    public int GetRoomCount()
    {
        return generatedRooms.Count;
    }
}