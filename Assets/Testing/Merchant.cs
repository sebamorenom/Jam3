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

    private void Awake()
    {
        FillBoxes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Weapon"))
        {
            curr.Exchange(transform.position + transform.forward, other.gameObject.GetComponent<Item>().price);
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
public class MerchantBox : MonoBehaviour
{
    public Merchant seller;
    public GameObject objInside;
    public int index;
    public float price;

    public void Fill(GameObject obj, int pos, Merchant mer)
    {
        obj.transform.parent = transform;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        seller = mer;
        Item auxItem = obj.GetComponent<Item>();
        price = auxItem.price;
        index = pos;
    }

    public void Sold()
    {
        objInside.transform.parent = null;
        objInside.GetComponent<Rigidbody>().isKinematic = false;
    }

}