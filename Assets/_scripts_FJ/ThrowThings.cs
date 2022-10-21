using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowThings : MonoBehaviour
{
    [SerializeField] Rigidbody axeRb;
    [SerializeField] float throwPower;
    public Axe axe;
    void Start()
    {
        
        axeRb = GetComponentInChildren<Rigidbody>();
    }

    
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            AxeThrow();
        }
    }

    private void AxeThrow()
    {
         
        
        axeRb.isKinematic = false;
        axeRb.transform.parent = null;
        axeRb.AddForce(Camera.main.transform.TransformDirection(Vector3.forward) * throwPower + transform.up * 2, ForceMode.Impulse);
        axeRb.AddTorque(axeRb.transform.TransformDirection(Vector3.right) * 100, ForceMode.Impulse);
    }
}
