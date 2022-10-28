using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemySpawnPool", menuName = "ScriptableObjects/SpawnableEnemyPool", order = 2)]
public class SpawnableEnemyPool : ScriptableObject
{
    [SerializeField]
    public GameObject[] enemies;
    [SerializeField]
    public float[] spawningCosts;
    // Start is called before the first frame update


    public void GetEnemies(ref GameObject[] spawningPoints)
    {
        float maxPerSpawn = spawningCosts[spawningCosts.Length - 1] - spawningPoints.Length;
        for (int i = 0; i < spawningPoints.Length; i++)
        {
            spawningPoints[i] = GetRandomEnemyForCost(maxPerSpawn);
        }
    }
    public GameObject GetRandomEnemyForCost(float cost)
    {
        int i = spawningCosts.Length - 1;
        while (spawningCosts[i] > cost)
        {
            i--;
        }
        return enemies[Random.Range(0, i + 1)];
    }

    //TO DO
}
