using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public Item item;

    [Header("Settings")]
    public int totalThrows;
    public float throwCoolDown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;
    void Start()
    {
        readyToThrow = true;
    }

    
    void Update()
    {
        if(Input.GetKey(throwKey) && readyToThrow && totalThrows > 0)
        {
            
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        //instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, Quaternion.identity);

        //get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        //calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;
        //implement ThrowCooldown
        Invoke(nameof(ResetThrow),throwCoolDown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
