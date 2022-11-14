using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    SpawnableEnemyPool pool;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Spawn()
    {
        Enemy auxEnemy = Instantiate(pool.GetRandomEnemyForProbability(), transform).GetComponent<Enemy>();
        auxEnemy.transform.parent = transform.parent;
    }
}
