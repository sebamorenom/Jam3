using Language.Lua;
using Opsive.UltimateInventorySystem.UI.Menus.Chest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    [NonSerialized]
    Item analyzedItem;
    [SerializeField]
    float scanDistance;
    [SerializeField]
    GameObject statsPanel;
    [SerializeField]
    Vector3 panelOffset;
    [SerializeField]
    TMPro.TextMeshProUGUI nameUI;
    [SerializeField]
    TMPro.TextMeshProUGUI damageTypeUI;
    [SerializeField]
    TMPro.TextMeshProUGUI valueUI;
    [SerializeField]
    Image valueComparisonLeft;
    [SerializeField]
    Image valueComparisonRight;
    [SerializeField]
    TMPro.TextMeshProUGUI priceUI;
    [SerializeField]
    TMPro.TextMeshProUGUI descriptionUI;
    [SerializeField]
    Sprite arrowBetter;
    [SerializeField]
    Sprite arrowWorst;
    [SerializeField]
    Sprite signEqual;

    private Item leftHandItem;
    private Item rightHandItem;

    private ChestUsable chestSelected;
    private MerchantBox boxSelected;
    private Item lastSeenItem;
    private Item toPickUp;
    private MerchantBox toBuy;
    private bool wantPickUp;
    private Wallet wallet;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(CheckForItem());
        wallet = GetComponent<Wallet>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            wantPickUp = true;
        }

    }

    IEnumerator CheckForItem()
    {
        for (; ; )
        {
            RaycastHit hit;
            if (Physics.BoxCast(transform.position, Vector3.one, Camera.main.transform.forward, out hit, Quaternion.identity, scanDistance, 1 << LayerMask.NameToLayer("Item")))
            {
                if (hit.collider.gameObject.TryGetComponent<ChestUsable>(out chestSelected))
                {
                    if (wantPickUp)
                    {
                        chestSelected.Open();
                        wantPickUp = false;
                    }

                }
                chestSelected = null;
                if (hit.collider.gameObject.TryGetComponent<MerchantBox>(out boxSelected))
                {
                    if (wantPickUp)
                    {
                        wallet.TryToBuy(boxSelected);
                        wantPickUp = false;
                    }
                    //lastSeenItem = boxSelected.objInside.GetComponent<Item>();
                    statsPanel.SetActive(true);
                    statsPanel.transform.position = boxSelected.transform.position + panelOffset;
                    statsPanel.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
                    nameUI.text = boxSelected.objName;
                    priceUI.text = boxSelected.price.ToString();
                    damageTypeUI.enabled = false;
                    valueUI.enabled = false;
                    descriptionUI.enabled = false;

                }

                else if (hit.collider.gameObject.TryGetComponent<Item>(out lastSeenItem))
                {
                    if (wantPickUp)
                    {
                        toPickUp = lastSeenItem;
                        wantPickUp = false;
                    }
                    analyzedItem = hit.collider.gameObject.GetComponent<Item>();
                    statsPanel.SetActive(true);
                    statsPanel.transform.position = analyzedItem.transform.position + panelOffset;
                    statsPanel.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

                    nameUI.text = "Name: " + analyzedItem.itemName;
                    priceUI.text = "Price: " + analyzedItem.price.ToString();
                    valueUI.enabled = true;
                    switch (analyzedItem.type)
                    {
                        case ItemType.Weapon:
                            damageTypeUI.enabled = true;
                            descriptionUI.enabled = false;
                            switch (analyzedItem.weapType)
                            {
                                case WeaponType.Shield:
                                    valueUI.text = "Protection: " + analyzedItem.value;
                                    damageTypeUI.enabled = false;
                                    break;
                                default:
                                    valueUI.text = "Damage: " + analyzedItem.value;
                                    damageTypeUI.text = analyzedItem.damageType + " damage";
                                    break;
                            }
                            CompareValue(analyzedItem);
                            break;

                        case ItemType.Equipment:
                            descriptionUI.enabled = false;
                            damageTypeUI.enabled = false;
                            valueUI.text = "Protection: " + analyzedItem.value;
                            break;

                        case ItemType.Consumable:
                            damageTypeUI.enabled = false;
                            descriptionUI.enabled = false;
                            switch (analyzedItem.consType)
                            {
                                case ConsumableType.HealDamage:
                                    if (analyzedItem.value < 0)
                                    {
                                        valueUI.text = "Deals " + analyzedItem.value + " damage";
                                    }
                                    else
                                    {
                                        valueUI.text = "Heals " + analyzedItem.value + " HP";
                                    }
                                    break;

                                case ConsumableType.BuffDebuff:
                                    valueUI.enabled = false;
                                    descriptionUI.enabled = true;
                                    descriptionUI.text = analyzedItem.BuffToText();
                                    break;

                            }
                            break;
                    }
                }
            }
            else
            {
                statsPanel.SetActive(false);
                wantPickUp = false;
                lastSeenItem = null;
                boxSelected = null;
            }
            yield return new WaitForSeconds(0.2f);
        }

    }

    public bool GetLastSeenItem(out GameObject aux, out WeaponType weapType, out bool flip)
    {
        flip = false;
        if (toPickUp != null)
        {
            flip = toPickUp.weapType == WeaponType.Shield ? true : false;
            weapType = toPickUp.weapType;
            aux = toPickUp.gameObject;
            toPickUp = null;
            return true;
        }
        else
        {
            weapType = WeaponType.None;
            aux = null;
            return false;
        }
    }


    public void SetItems(Item _leftHand, Item _rightHand)
    {
        leftHandItem = _leftHand;
        rightHandItem = _rightHand;
    }

    public void CompareValue(Item pickableItem)
    {
        if (leftHandItem != null && leftHandItem.weapType == analyzedItem.weapType && leftHandItem.weapType == WeaponType.Shield)
        {
            if (analyzedItem.value > leftHandItem.value)
            {
                valueComparisonLeft.sprite = arrowBetter;
            }
            if (analyzedItem.value == leftHandItem.value)
            {
                valueComparisonLeft.sprite = signEqual;
            }
            if (analyzedItem.value < leftHandItem.value)
            {
                valueComparisonLeft.sprite = arrowWorst;
            }
        }
        else if (leftHandItem != null && leftHandItem.weapType != WeaponType.Shield && analyzedItem.weapType != WeaponType.Shield)
        {
            if (analyzedItem.value > leftHandItem.value)
            {
                valueComparisonLeft.sprite = arrowBetter;
            }
            if (analyzedItem.value == leftHandItem.value)
            {
                valueComparisonLeft.sprite = signEqual;
            }
            if (analyzedItem.value < leftHandItem.value)
            {
                valueComparisonLeft.sprite = arrowWorst;
            }
        }
        else
        {
            valueComparisonLeft.sprite = signEqual;

        }

        if (rightHandItem != null && rightHandItem.weapType == analyzedItem.weapType && rightHandItem.weapType == WeaponType.Shield)
        {
            if (analyzedItem.value > rightHandItem.value)
            {
                valueComparisonRight.sprite = arrowBetter;
            }
            if (analyzedItem.value == rightHandItem.value)
            {
                valueComparisonRight.sprite = signEqual;
            }
            if (analyzedItem.value < rightHandItem.value)
            {
                valueComparisonRight.sprite = arrowWorst;
            }
        }
        else if (rightHandItem != null && rightHandItem.weapType != WeaponType.Shield && analyzedItem.weapType != WeaponType.Shield)
        {
            if (analyzedItem.value > rightHandItem.value)
            {
                valueComparisonRight.sprite = arrowBetter;
            }
            if (analyzedItem.value == rightHandItem.value)
            {
                valueComparisonRight.sprite = signEqual;
            }
            if (analyzedItem.value < rightHandItem.value)
            {
                valueComparisonRight.sprite = arrowWorst;
            }
        }
        else
        {
            valueComparisonRight.sprite = signEqual;
        }
    }

}
/*
    public void GetHoldingItems(ref Item leftHand, ref Item rightHand)
    {
        leftHand=leftHandWeapon;
        righHand=rightHandWeapon;
    }
 */