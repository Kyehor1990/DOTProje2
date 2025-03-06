using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Zindandan çıktın!");
        }
    }
}
