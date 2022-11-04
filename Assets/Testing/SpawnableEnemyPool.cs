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
        for (int i = 0; i < spawningPoints.Length; i++)
        {
            spawningPoints[i] = Instantiate(GetRandomEnemyForProbability());
        }
    }
    public GameObject GetRandomEnemyForProbability()
    {
        float sumProb = 0 + roomCount / 100;
        float randNumb = Random.value;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (spawningProbabilities[i] / 100 + sumProb >= randNumb)
            {
                return enemies[i];
            }
            sumProb += spawningProbabilities[i] / 100;
        }
        return null;
    }

    public void SetCurrentRoom(int _roomCount)
    {
        roomCount = _roomCount;
    }

}
