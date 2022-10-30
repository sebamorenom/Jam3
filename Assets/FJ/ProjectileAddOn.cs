using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddOn : MonoBehaviour
{
    private Rigidbody rb;
    private bool targetHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();    
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
        

        //make sure projectile moves with target
        transform.SetParent(collision.transform);

        Destroy(gameObject, 5f);
    }
}
