using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// A collection of the Nav Mesh Surfaces this tries to get.
    /// </summary>
    [SerializeField]
    NavMeshSurface navMeshSurface;
    #endregion

    void Start()
    {
        navMeshSurface.BuildNavMesh();
    }
    
    void Update()
    {
        
    }
}