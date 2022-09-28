using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]

public class NavMeshManager : MonoBehaviour
{
    private NavMeshSurface[] Surfaces;

    private static NavMeshManager _Instance;

    public static NavMeshManager Instance
    {
        get
        {
            return _Instance;
        }

        private set
        {
            _Instance = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Multiple NavMeshManagers in the scene! Destroying {name}!");
            Destroy(gameObject);
            return;
        }

        Surfaces = GetComponents<NavMeshSurface>();
        Instance = this;
    }

    public void BakeNavMesh()
    {
        foreach (NavMeshSurface surface in Surfaces)
        {
            surface.BuildNavMesh();
        }
    }
}
