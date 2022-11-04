using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    SpawnableEnemyPool pool;
    [SerializeField]
    GameObject player;
    public bool testingSpawn;
    private float availablePoints;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (testingSpawn)
        {
            Spawn();
            testingSpawn = false;
        }
    }

    public void Spawn()
    {
        Enemy auxEnemy = Instantiate(pool.GetRandomEnemyForProbability()).GetComponent<Enemy>();
        auxEnemy.player = player;
    }

    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }

}
