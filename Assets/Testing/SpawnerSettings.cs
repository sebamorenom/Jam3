using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSettings : MonoBehaviour
{
    [SerializeField]
    Spawner[] spawners;

    // Start is called before the first frame update
    void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ActivateSpawners()
    {
        foreach (Spawner spawn in spawners)
        {
            spawn.Spawn();
        }
    }
}
