using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    private List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void EnemyDefeated(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            Door[] doors = FindObjectsOfType<Door>();
            foreach (Door door in doors)
            {
                door.UnlockDoor();
            }
        }
    }
}
