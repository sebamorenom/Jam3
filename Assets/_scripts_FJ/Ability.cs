using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string name;
    public float coolDown;
    public float activeTime;


    public virtual void Activate(GameObject parent)
    {

    }
}
