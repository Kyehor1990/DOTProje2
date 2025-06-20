using UnityEngine;
using NavMeshPlus.Components;

public class RoomNavMeshBaker : MonoBehaviour
{
    private NavMeshSurface surface;

    void Awake()
    {
        surface = FindObjectOfType<NavMeshSurface>(); // Tek bir tane varsa
    }

    void Start()
    {
        surface.BuildNavMesh();
    }
}
