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
    public float[] spawningProbabilities;

    private int roomCount = 0;
    // Start is called before the first frame update


    public void GetEnemies(ref GameObject[] spawningPoints)
    {
        float maxPerSpawn = spawningProbabilities[spawningProbabilities.Length - 1] - spawningPoints.Length;
        for (int i = 0; i < spawningPoints.Length; i++)
        {
            spawningPoints[i] = Instantiate(GetRandomEnemyForProbability(maxPerSpawn));
        }
    }
    public GameObject GetRandomEnemyForProbability(float cost)
    {
        float sumProb = 0 + roomCount / 100;
        float randNumb = Random.value;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (spawningProbabilities[i] + sumProb >= randNumb)
            {
                return enemies[i];
            }
            sumProb += spawningProbabilities[i];
        }
        return null;
    }

    public void SetCurrentRoom(int _roomCount)
    {
        roomCount = _roomCount;
    }

}
