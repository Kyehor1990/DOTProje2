using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject roomPrefab;
    public Energy _energy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnterNewRoom()
    {
        _energy.ReduceEnergy();

        if (_energy.energy > 0)
        {
            Debug.Log("Player entered a new room.");
        }
        else
        {
            Debug.Log("Game Over: No energy left.");
        }
    }
}