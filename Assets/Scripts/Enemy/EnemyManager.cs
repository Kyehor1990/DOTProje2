using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<GameObject> enemies = new List<GameObject>();

    public PlayerEnergy playerEnergy;

    public AudioSource audioSource;
    public AudioClip doorSound;
    public SceneChange SceneChange;
    public bool dungeonExit = true;

    void Awake()
    {
        if (instance == null) instance = this;
        dungeonExit = true;
    }

    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
        dungeonExit = false;
    }

    public void EnemyDefeated(GameObject enemy)
    {
        enemies.Remove(enemy);
        CheckEnemies();
    }

    public void CheckEnemies()
    {
        Debug.Log("Kalan düşman sayısı: " + enemies.Count);
        if (enemies.Count == 0)
        {
            dungeonExit = true;
            if (SceneChange.Dungeon)
                audioSource.PlayOneShot(doorSound);
            if (playerEnergy.currentEnergy <= 0)
            {
                Debug.LogError("Enerji KODU");
                PlayerEnergy.instance.ForceExitDungeon();
            }

            Door[] doors = FindObjectsOfType<Door>();
            foreach (Door door in doors)
            {
                door.UnlockDoor();
            }
        }
    }
}
