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
        if(collision.gameObject.layer == 27)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponentInChildren<VisualEffect>().Play();
            Destroy(gameObject, 0.5f);
            Vector3 aux = collision.GetContact(0).point;
            collision.gameObject.GetComponent<Enemy>().TakeDamage(spell.damage,aux);
        }
        
    }
}
