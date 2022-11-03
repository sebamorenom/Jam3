using JAM3.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddOn : MonoBehaviour
{
    private Rigidbody rb;
    private bool targetHit;
    private Item item;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        item = GetComponent<Item>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        //make sure only to stick to the first target you hit
        if (targetHit)
            return;
        else
            targetHit = true;

        //make sure projectile sticks to surface
        rb.isKinematic = true;
        //collision.gameObject.GetComponent<Health>().TakeDamage(item.d);
        

        //make sure projectile moves with target
        transform.SetParent(collision.transform);

        Destroy(gameObject, 5f);
    }
}
