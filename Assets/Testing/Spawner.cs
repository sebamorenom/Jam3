using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    SpawnableEnemyPool pool;
    private float availablePoints;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAvailablePoints(float points)
    {
        availablePoints = points;
    }
    public void Spawn()
    {
        //Instantiate(pool.GetEnemy(availablePoints));
    }

}
