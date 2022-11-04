using PixelCrushers.QuestMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Wallet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float quantity;
    [NonSerialized]
    GameObject money;
    public Currency curr;
    [SerializeField]
    Transform nearCheckerPos;
    [SerializeField]
    TMPro.TextMeshProUGUI moneyShower;
    private bool wantsToBuy;

    public void OnDeath()
    {
        DropMoney();
    }

    public void Start()
    {
        if (nearCheckerPos != null)
            StartCoroutine(CheckNear());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            wantsToBuy = true;
        }
        ShowMoney();
    }

    public void FixedUpdate()
    {

    }


    IEnumerator CheckNear()
    {
        for (; ; )
        {
            foreach (Collider col in Physics.OverlapSphere(nearCheckerPos.position, 6, 1 << LayerMask.NameToLayer("Money")))
            {
                Rigidbody aux = col.gameObject.GetComponent<Rigidbody>();
                aux.AddForce((transform.position - col.transform.position).normalized * 300, ForceMode.Acceleration);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void DropMoney()
    {
        curr.Exchange(transform.position, quantity);
    }
    public void TakeMoney(float money)
    {
        quantity += money;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Money"))
        {
            TakeMoney(other.gameObject.GetComponent<Money>().value);
            Destroy(other.gameObject);
        }
    }

    public bool TryToBuy(MerchantBox itemBox)
    {
        Merchant aux = itemBox.seller;
        return aux.CheckEnough(ref quantity, itemBox.index);
    }

    public void ShowMoney()
    {
        // moneyShower.text = quantity.ToString();
    }

}
