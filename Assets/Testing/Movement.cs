using DunGen.Demo;
using MeshCombineStudio;
using Newtonsoft.Json;
using Opsive.UltimateInventorySystem.DropsAndPickups;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;



public class Movement : Entity
{
    private enum HandSituation { Free, RightOcc, LeftOcc, BothOcc }

    Vector3 inputVect;
    Vector3 movVect;
    Vector2 camMov;
    Vector2 camRot;
    [Header("Movement Parameters")]
    [SerializeField]
    float movementSpeed;
    [Header("Camera Parameters")]
    [SerializeField]
    Transform camTransform;
    [SerializeField]
    float mouseSensitivity;
    [Header("Utilities")]
    [SerializeField]
    Item hands;
    [SerializeField]
    GameObject agaDer;
    [SerializeField]
    GameObject agaIzq;
    [SerializeField]
    GameObject shieldPos;
    [SerializeField]
    CollSwitcher[] collSwitchers;
    [SerializeField]
    Animator[] handAnimators;


    private HandSituation hSituation = HandSituation.Free;
    private Animator anim;
    private Scanner scan;
    public Rigidbody rb;
    private GameObject agaDerChild;
    private GameObject agaIzqChild;
    private Item weaponRight;
    private Item weaponLeft;
    public bool wantsThrowLeft = false;
    public bool wantsThrowRight = false;
    public bool wantsPrimaryAction = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 currentRot = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        scan = GetComponent<Scanner>();
        StartCoroutine(CheckLastSeenObject());
        anim = GetComponent<Animator>();
        rb.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        GetCameraInputs();
        MoveCamera();
        GetPrimaryAttackInput();
        GetThrowingInputs();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        CheckWeaponHits();
        CheckThrowInput();
        ClearInputs();
    }

    public void MoveCamera()
    {
        camRot.x += camMov.x;
        camRot.y -= camMov.y;
        camRot.y = Mathf.Clamp(camRot.y, -90, 90);
        transform.eulerAngles = Vector3.up * camRot.x;
        camTransform.localEulerAngles = Vector3.right * camRot.y;
    }

    public void MoveCharacter()
    {
        Vector3 speed = movVect * movementSpeed;
        rb.AddForce(speed, ForceMode.Force);
    }

    IEnumerator CheckLastSeenObject()
    {
        GameObject aux = null;
        WeaponType auxWeapType = WeaponType.None;
        for (; ; )
        {
            bool flip;
            scan.GetLastSeenItem(out aux, out auxWeapType, out flip);
            if (aux != null)
            {
                Debug.Log(hSituation);
                if (agaDer.transform.childCount == 0)
                {
                    switch (hSituation)
                    {
                        case HandSituation.Free:
                            hSituation = auxWeapType == WeaponType.TwoHanded ? HandSituation.BothOcc : HandSituation.RightOcc;
                            if (auxWeapType == WeaponType.TwoHanded)
                            {
                                handAnimators[0].SetBool("TwoHanding", true);
                                handAnimators[1].SetBool("TwoHanding", true);
                                handAnimators[0].SetInteger("LHandWeaponType", (int)auxWeapType);
                                handAnimators[1].SetInteger("LHandWeaponType", (int)auxWeapType);
                            }
                            if (auxWeapType == WeaponType.Shield)
                            {
                                handAnimators[0].SetInteger("LHandWeaponType", (int)auxWeapType);
                                handAnimators[1].SetInteger("LHandWeaponType", (int)auxWeapType);
                                Parent(shieldPos.transform, aux.transform, flip);
                            }
                            else
                            {
                                handAnimators[0].SetInteger("RHandWeaponType", (int)auxWeapType);
                                handAnimators[1].SetInteger("RHandWeaponType", (int)auxWeapType);
                                Parent(agaDer.transform, aux.transform, flip);
                            }
                            break;
                        case HandSituation.LeftOcc:
                            if (auxWeapType != WeaponType.TwoHanded)
                            {
                                switch (auxWeapType)
                                {
                                    case WeaponType.Shield:
                                        if (handAnimators[0].GetInteger("LHandWeaponType") != (int)WeaponType.Shield)
                                        {
                                            GameObject auxGameObj = agaIzq.transform.GetChild(0).gameObject;
                                            Parent(shieldPos.transform, aux.transform, flip);
                                            Parent(agaDer.transform, auxGameObj.transform, false);
                                            handAnimators[0].SetInteger("RHandWeaponType", handAnimators[0].GetInteger("LHandWeaponType"));
                                            handAnimators[1].SetInteger("RHandWeaponType", handAnimators[0].GetInteger("LHandWeaponType"));
                                            handAnimators[0].SetInteger("LHandWeaponType", (int)WeaponType.Shield);
                                            handAnimators[1].SetInteger("LHandWeaponType", (int)WeaponType.Shield);
                                        };
                                        break;
                                    default:
                                        hSituation = HandSituation.BothOcc;
                                        handAnimators[0].SetInteger("RHandWeaponType", (int)auxWeapType);
                                        handAnimators[1].SetInteger("RHandWeaponType", (int)auxWeapType);
                                        Parent(agaDer.transform, aux.transform, flip);
                                        break;
                                }
                            }
                            break;
                    }
                }
                else if (agaIzq.transform.childCount == 0 && hSituation != HandSituation.BothOcc && shieldPos.transform.childCount == 0)
                {
                    if (hSituation == HandSituation.RightOcc)
                    {
                        if (auxWeapType != WeaponType.TwoHanded)
                        {
                            hSituation = HandSituation.BothOcc;
                            handAnimators[0].SetInteger("LHandWeaponType", (int)auxWeapType);
                            handAnimators[1].SetInteger("LHandWeaponType", (int)auxWeapType);
                            if (auxWeapType != WeaponType.Shield)
                            {
                                Parent(agaIzq.transform, aux.transform, flip);
                            }
                            else
                            {
                                Parent(shieldPos.transform, aux.transform, flip);
                            }
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void Parent(Transform parent, Transform child, bool flip)
    {
        child.parent = parent;
        if (parent == agaDer.transform)
        {
            collSwitchers[0].SetCollider(child.gameObject.GetComponent<Item>().hitColliders);
            agaDerChild = child.gameObject;
            weaponRight = child.GetComponentInChildren<Item>();
        }
        else
        {
            collSwitchers[1].SetCollider(child.gameObject.GetComponent<Item>().hitColliders);
            agaIzqChild = child.gameObject;
            weaponLeft = child.GetComponentInChildren<Item>();
        }
        child.GetComponent<Rigidbody>().isKinematic = true;
        child.position = parent.position;
        child.localRotation = Quaternion.identity;
        child.GetComponentInChildren<MeshCollider>().enabled = false;
    }

    public void CheckThrowInput()
    {
        if (agaDer.transform.childCount != 0 && wantsThrowRight)
        {
            Throw(true);
            wantsThrowRight = false;
        }
        if ((agaIzq.transform.childCount != 0 || shieldPos.transform.childCount != 0) && wantsThrowLeft)
        {
            Throw(false);
            wantsThrowLeft = false;
        }
        if (wantsPrimaryAction)
        {
            handAnimators[0].SetTrigger("Primary");
            handAnimators[1].SetTrigger("Primary");
            wantsPrimaryAction = false;
        }
    }

    public void CheckWeaponHits()
    {
        Entity aux;
        Vector3 auxPos;
        if (weaponRight != null)
        {
            if (weaponRight.HasHit(out aux, out auxPos))
            {
                Enemy auxEnemy = (Enemy)aux;
                auxEnemy.TakeDamage(strength + weaponRight.value, auxPos);
            }
        }
        if (weaponLeft != null)
        {
            if (weaponLeft.HasHit(out aux, out auxPos))
            {
                Enemy auxEnemy = (Enemy)aux;
                auxEnemy.TakeDamage(strength + weaponLeft.value, auxPos);
            }
        }

    }

    public void Throw(bool leftRight)
    {
        Transform aux = null;
        aux = leftRight ? agaDerChild.transform : agaIzqChild.transform.childCount != 0 ? agaIzqChild.transform : shieldPos.transform.GetChild(0);
        if (leftRight)
        {
            agaDerChild = null;
            weaponRight = null;
        }
        else
        {
            agaIzqChild = null;
            weaponLeft = null;
        }
        aux.parent = null;
        Rigidbody rgb = aux.GetComponent<Rigidbody>();
        MeshCollider meshColl = aux.GetComponentInChildren<MeshCollider>();
        meshColl.enabled = true;
        rgb.isKinematic = false;
        rgb.AddForce(Camera.main.transform.forward * 9, ForceMode.Impulse);
        rgb.AddForceAtPosition(Camera.main.transform.forward * 6f, aux.position + 3f * aux.up, ForceMode.Impulse);
        if (handAnimators[0].GetInteger("LHandWeaponType") == (int)WeaponType.TwoHanded && handAnimators[0].GetInteger("RHandWeaponType") == (int)WeaponType.TwoHanded)
        {
            handAnimators[0].SetInteger("LHandWeaponType", (int)WeaponType.Fist);
            handAnimators[1].SetInteger("LHandWeaponType", (int)WeaponType.Fist);
            handAnimators[0].SetInteger("RHandWeaponType", (int)WeaponType.Fist);
            handAnimators[1].SetInteger("RHandWeaponType", (int)WeaponType.Fist);
            handAnimators[0].SetBool("TwoHanding", false);
            handAnimators[1].SetBool("TwoHanding", false);

            hSituation = HandSituation.Free;
        }
        if (leftRight)
        {
            if (handAnimators[0].GetInteger("LHandWeaponType") != (int)WeaponType.Fist)
            {
                hSituation = HandSituation.LeftOcc;
            }
            else
            {
                hSituation = HandSituation.Free;
            }
            handAnimators[0].SetInteger("RHandWeaponType", (int)WeaponType.Fist);
            handAnimators[1].SetInteger("RHandWeaponType", (int)WeaponType.Fist);
        }
        else
        {
            if (handAnimators[0].GetInteger("RHandWeaponType") != (int)WeaponType.Fist)
            {
                hSituation = HandSituation.RightOcc;
            }
            else
            {
                hSituation = HandSituation.Free;
            }
            handAnimators[0].SetInteger("LHandWeaponType", (int)WeaponType.Fist);
            handAnimators[1].SetInteger("LHandWeaponType", (int)WeaponType.Fist);
        }
    }

    public void GetInputs()
    {
        inputVect = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movVect = inputVect.x * transform.right + inputVect.z * transform.forward;
    }

    public void GetCameraInputs()
    {
        camMov = new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity, Input.GetAxis("Mouse Y") * mouseSensitivity);
    }

    public void GetPrimaryAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            wantsPrimaryAction = true;
        }
    }

    public void GetThrowingInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            wantsThrowLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            wantsThrowRight = true;
        }
    }

    public void CalculateCurrentDamage()
    {

    }

    public void CorrectChildRotation()
    {
        Vector3 pointUp = Vector3.up * 90;
        switch (hSituation)
        {
            case HandSituation.Free:
                break;
            case HandSituation.RightOcc:
                agaDerChild.transform.rotation = Quaternion.Euler(pointUp);
                break;
            case HandSituation.LeftOcc:
                agaIzqChild.transform.rotation = handAnimators[0].GetInteger("LHandWeaponType") == 6 ? Quaternion.Euler(-pointUp) : Quaternion.Euler(pointUp);
                break;
            case HandSituation.BothOcc:
                agaDerChild.transform.rotation = Quaternion.Euler(pointUp);
                agaIzqChild.transform.rotation = handAnimators[0].GetInteger("LHandWeaponType") == 6 ? Quaternion.Euler(-pointUp) : Quaternion.Euler(pointUp);
                break;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            Debug.Log("Hasta aquí bien");
            bool meleeHit = false;
            Vector3 meleePoint = Vector3.zero;
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).otherCollider.gameObject.layer == LayerMask.NameToLayer("HitColliders"))
                {
                    meleeHit = true;
                    meleePoint = collision.GetContact(i).point;
                }
            }
            if (meleeHit)
            {
                collision.collider.GetComponent<Enemy>().TakeDamage(strength, meleePoint);
            }
        }

    }

    public void ClearInputs()
    {
        wantsPrimaryAction = false;
        wantsThrowRight = false;
        wantsThrowLeft = false;
    }
}
