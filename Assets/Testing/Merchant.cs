using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    MerchantStash stash;
    [SerializeField]
    Currency curr;
    [SerializeField]
    MerchantBox[] boxes;


    private Collider coll;
    private void Start()
    {
        FillBoxes();
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            curr.Exchange(coll.bounds.center + transform.up * 2, other.gameObject.GetComponent<Item>().price);
            Destroy(other.gameObject);
        }
    }



    private void FillBoxes()
    {
        GameObject[] aux = stash.GetRandomItems(boxes.Length);
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].Fill(aux[i], i, this);
        }
    }

    public bool CheckEnough(ref float quantity, int boxIndex)
    {
        if (quantity >= boxes[boxIndex].price)
        {
            quantity -= boxes[boxIndex].price;
            boxes[boxIndex].Sold();
            return true;
        }
        else
        {
            return false;
        }
    }



}

