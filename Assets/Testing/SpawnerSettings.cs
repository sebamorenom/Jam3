using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSettings : MonoBehaviour
{
    [SerializeField]
    AnimationCurve animCurve;
    [SerializeField]
    bool test;    
    //PlayerHelper pHelper;
    [SerializeField]
    Spawner[] spawners;

    public float difficulty;
    // Start is called before the first frame update
    void OnEnable()
    {
        ProcessCurve();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ProcessCurve()
    {
        //difficulty=Mathf.Ceil(animCurve.Evaluate(pHelper.tilesVisited)*20);
    }

    /*
    public void SetPlayer(PlayerHelper play)
    { 
        pHelper=play;
        gameObject.active=true;
    }*/

    public void ActivateSpawners()
    {
        foreach(Spawner spawn in spawners)
        {
            spawn.SetAvailablePoints(difficulty);
            spawn.Spawn();
        }
    }
}
