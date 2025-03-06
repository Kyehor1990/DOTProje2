using UnityEngine;

public class Energy : MonoBehaviour
{
    public int energy = 10; // Starting energy

    public void ReduceEnergy()
    {
        energy--;
        Debug.Log("Energy reduced. Current energy: " + energy);

        if (energy <= 0)
        {
            Debug.Log("No energy left. Cannot enter new rooms.");
        }
    }
}