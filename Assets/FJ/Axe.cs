using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(collision.transform);
    }
}
