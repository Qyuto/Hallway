using System;
using UnityEngine;
using UnityEngine.AI;

namespace Rooms
{
    public class NavMeshBaker : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface surface;

        private void Awake()
        {
            surface.BuildNavMesh();
        }
    }
}