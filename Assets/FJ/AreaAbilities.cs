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
        
        Collider[] objectives = Physics.OverlapSphere(spawnPosition, areaRadius, 9);
        foreach(Collider c in objectives)
        {
            
        }
    }
}
