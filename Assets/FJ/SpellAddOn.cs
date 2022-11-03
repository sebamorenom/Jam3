using JAM3.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpellAddOn : MonoBehaviour
{
     public Spells spell;
    
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponentInChildren<VisualEffect>().Play();
        Destroy(gameObject, 0.5f);
        collision.gameObject.GetComponent<Entity>().TakeDamage(spell.damage);
    }
}
