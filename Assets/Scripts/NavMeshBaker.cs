using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshSurface navSurface;
    void Start()
    {
        navSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
