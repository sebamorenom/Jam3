using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshSurface navSurface;
    void OnStart()
    {
        navSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
