using System.Collections;
using JAM3.Health;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBomb : MonoBehaviour
{
    public float areaRadius;
    Collider[] objectives;
    public Spells spell;
    public float duration;


    void Start()
    {
        
    }

    
    void Update()
    {
        foreach (Collider c in objectives)
        {
            if (c.CompareTag("Entity") && duration > 0)
            {
                c.gameObject.GetComponent<Health>().currentHealth -= spell.damage * Time.deltaTime;
                
            }
        }
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            
            foreach(Collider c in objectives)
            {
                c.gameObject.GetComponent<Health>().currentHealth = c.gameObject.GetComponent<Health>().currentHealth;
            }
            
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 spawnPosition = collision.contacts[0].point;


        objectives = Physics.OverlapSphere(spawnPosition, areaRadius);
        Destroy(this.gameObject,duration);


    }
}
