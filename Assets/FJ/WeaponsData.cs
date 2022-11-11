using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Weapons", menuName ="JAM/WeaponsData",order = 1)]
public class WeaponsData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public int weaponDamage;
    public bool leftHanded;
    public bool throwable;
    public Sprite icon;
}
