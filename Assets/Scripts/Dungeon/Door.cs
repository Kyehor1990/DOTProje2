using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public DoorPosition doorPosition;
    public string roomType = "1";
    public bool StartDoor = false;

    [Header("Door Sprites")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    private bool isLocked = true;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning($"Door {gameObject.name} üzerinde SpriteRenderer bulunamadı!");
            return;
        }

        // Starting door ise baştan açık olsun
        if (StartDoor)
        {
            UnlockDoor();
        }
        else
        {
            LockDoor();
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;

        if (spriteRenderer != null && openSprite != null)
        {
            spriteRenderer.sprite = openSprite;
        }

        Debug.Log($"Kapı açıldı: {doorPosition}");
    }

    public void LockDoor()
    {
        isLocked = true;

        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isLocked)
            return;

        if (DungeonManager.instance == null)
        {
            Debug.LogError("DungeonManager instance bulunamadı!");
            return;
        }

        Vector3 newRoomPos = DungeonManager.instance.GetNewRoomPositionFromDoor(doorPosition);
        bool roomAlreadyExists = DungeonManager.instance.DoesRoomExistAt(newRoomPos);

        // Oda zaten varsa veya yeterli enerji varsa geçişe izin ver
        bool canEnter = roomAlreadyExists || 
                       (PlayerEnergy.instance != null && PlayerEnergy.instance.currentEnergy > 0);

        if (canEnter)
        {
            Debug.Log($"Kapıdan geçiliyor: {doorPosition} -> {roomType}");
            DungeonManager.instance.EnterRoom(roomType, doorPosition);
        }
        else
        {
            Debug.Log("Yeterli enerji yok veya geçiş yapılamıyor!");
        }
    }

    public void ConvertToExit()
    {
        roomType = "Exit";
        Debug.Log($"Bu kapı artık çıkış kapısı: {doorPosition}");
    }

    // Debug için - Inspector'da görmek isterseniz
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isLocked ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}