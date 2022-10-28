using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowThings : MonoBehaviour
{
    [SerializeField] Rigidbody axeRb;
    [SerializeField] float throwPower;
    public Transform target, curvePoint;
    private Vector3 oldPos;
    private bool isReturning = false;
    public Axe axe;
    public float time = 0.0f;
    void Start()
    {
        
        
    }

    
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            AxeThrow();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            AxeReturn();
        }
        if(isReturning)
        {
            if(time < 1.0f)
            {
                axeRb.position = GetBezierQuadraticCurvePoint(time, oldPos,curvePoint.position, target.position);
                axeRb.rotation = Quaternion.Slerp(axe.transform.rotation, target.rotation, 50 * Time.deltaTime);
                time += Time.deltaTime;
            }
            else
            {
                ResetAxe();
            }
        }
    }

    private void AxeThrow()
    {
        isReturning = false;
        axeRb.isKinematic = false;
        axeRb.transform.parent = null;
        axeRb.AddForce(Camera.main.transform.TransformDirection(Vector3.forward) * throwPower + transform.up * 2, ForceMode.Impulse);
        axeRb.AddTorque(axeRb.transform.TransformDirection(Vector3.right) * 100, ForceMode.Impulse);
    }

    private void AxeReturn()
    {
        time = 0.0f;
        oldPos = axeRb.position;
        isReturning = true;
        axeRb.velocity = Vector3.zero;
        axeRb.isKinematic = true;
        
    }
    
    private void ResetAxe()
    {
        isReturning = false;
        axeRb.transform.parent = transform;
        axeRb.position = target.position;
        axeRb.rotation = target.rotation;
    }

    Vector3 GetBezierQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }
}
