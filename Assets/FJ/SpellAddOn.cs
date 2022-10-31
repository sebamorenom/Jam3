using JAM3.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAddOn : MonoBehaviour
{
     public Spells spell;
    
    private void OnCollisionEnter(Collision collision)
    {
        GetComponentInChildren<ParticleSystem>().Play();
        Destroy(gameObject, 0.5f);
        collision.gameObject.GetComponent<Health>().TakeDamage(spell.damage);
    }
}
