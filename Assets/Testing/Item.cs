using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Debug = UnityEngine.Debug;

public enum ItemType { Weapon, Equipment, Consumable }
public enum WeaponType { None, Fist, Dagger, Axe, Sword, Mace, Shield, TwoHanded, Spear }
public enum EquipmentType { None, Armor, Amulet }
public enum ConsumableType { None, HealDamage, BuffDebuff };
public enum DamageType { None, Slashing, Blunt };

public class Item : MonoBehaviour
{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public ItemType type;
    [SerializeField]
    public WeaponType weapType;
    [SerializeField]
    public DamageType damageType;
    [SerializeField]
    public ConsumableType consType;
    [SerializeField]
    public string[] statsToBuff;
    [SerializeField]
    public float[] statsModifiers;
    [SerializeField]
    float splashRadius;
    [SerializeField]
    public float value;
    [SerializeField]
    public float price;
    [SerializeField]
    public string description;
    [SerializeField]
    public Collider[] hitColliders;

    private bool isHitting;
    bool isEquipped = false;
    MeshCollider meshColl;
    BoxCollider hitColl;
    Entity entityHurt;
    Vector3 hurtingPos;

    public void Start()
    {
        meshColl = GetComponent<MeshCollider>();
        hitColl = GetComponent<BoxCollider>();
    }
    // Update is called once per frame

    public void PrimaryAction(Animator anim)
    {
        anim.SetTrigger("Primary");

    }

    public void SecondaryAction(Animator anim)
    {
        anim.SetTrigger("Secondary");

    }

    private void Update()
    {
    }

    public void Drink()
    {
        //Humanoid aux = GetComponent<Entity>();
        switch (consType)
        {
            case ConsumableType.None:
                break;
            case ConsumableType.HealDamage:
                GetComponentInParent<Entity>().health += value;
                break;
            case ConsumableType.BuffDebuff:
                GetComponentInParent<Entity>().BuffDebuff(statsToBuff, statsModifiers, 5);
                break;
        }
    }

    public void Equip()
    {
        meshColl.enabled = false;
        isEquipped = true;
    }
    public void Unequip()
    {
        meshColl.enabled = false;
        isEquipped = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (type)
        {
            case ItemType.Consumable:
                if (collision.gameObject.layer == (1 << LayerMask.NameToLayer("Entity") | 1 << LayerMask.NameToLayer("Level")))
                {
                    Collider[] affectedArray = Physics.OverlapSphere(transform.position, splashRadius, 1 << LayerMask.NameToLayer("Entity"));
                    for (int i = 0; i < affectedArray.Length; i++)
                    {
                        StartCoroutine(affectedArray[i].GetComponent<Entity>().BuffDebuff(statsToBuff, statsModifiers, 5));
                    }
                }
                break;
            case ItemType.Weapon:
                if (collision.gameObject.layer == LayerMask.NameToLayer("Entity"))
                {
                    entityHurt = collision.collider.GetComponent<Entity>();
                }
                break;
        }

    }

    public string BuffToText()
    {
        string finalList = "";
        for (int i = 0; i < statsToBuff.Length; i++)
        {
            finalList += statsModifiers[i].ToString("+#;-#") + statsToBuff[i] + "\n";
        }
        return finalList.TrimEnd();
    }

    public bool HasHit(out Entity hitEntity, out Vector3 hitPoint)
    {
        if (entityHurt)
        {
            hitEntity = entityHurt;
            hitPoint = hurtingPos;
            entityHurt = null;
            hurtingPos = Vector3.zero;
            return true;
        }
        else
        {
            hitEntity = null;
            hitPoint = Vector3.zero;
            return false;
        }
    }

    public bool HasHit(out Entity hitEntity)
    {
        if (entityHurt)
        {
            hitEntity = entityHurt;
            entityHurt = null;
            hurtingPos = Vector3.zero;
            return true;
        }
        else
        {
            hitEntity = null;
            return false;
        }
    }

    public void SwitchHitColliders()
    {
        foreach (Collider coll in hitColliders)
        {
            coll.enabled = !coll.enabled;
        }
    }
}
