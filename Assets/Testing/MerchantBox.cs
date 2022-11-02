using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantBox : MonoBehaviour
{
    public Merchant seller;
    public GameObject objInside;
    public int index;
    public float price;
    private Collider[] objColliders;

    public void Fill(GameObject obj, int pos, Merchant mer)
    {
        obj.transform.parent = transform;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        objInside.GetComponent<Rigidbody>().useGravity = false;
        objInside = obj;
        seller = mer;
        Item auxItem = obj.GetComponent<Item>();
        price = auxItem.price;
        index = pos;
        objColliders = obj.GetComponents<Collider>();
        foreach (Collider coll in objColliders)
        {
            coll.enabled = false;
        }
    }

    public void Sold()
    {
        objInside.transform.parent = null;
        objInside.GetComponent<Rigidbody>().isKinematic = false;
        objInside.GetComponent<Rigidbody>().useGravity = true;
        foreach (Collider coll in objColliders)
        {
            coll.enabled = true;
        }
    }

}