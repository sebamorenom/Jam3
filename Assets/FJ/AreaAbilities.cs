using JAM3.Health;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "JAM/Area")]
public class AreaAbilities : Ability
{
    public float areaRadius;
    public float duration;
    public float damage;
    public float timeBetweenTicks;

    Vector3 spawnPosition;
    

    public override void Activate(GameObject parent)
    {
        RaycastHit hit;
         if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            spawnPosition = hit.point;
        }
        
        
    }


}
