using DunGen.Demo;
using MeshCombineStudio;
using Newtonsoft.Json;
using Opsive.UltimateInventorySystem.DropsAndPickups;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;

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
    [SerializeField]
    float jumpForce;
    [SerializeField]
    AnimationCurve accelCurve;
    [SerializeField]
    AnimationCurve deccelCurve;
    [NonSerialized]
    float timeAccelBegins;
    [NonSerialized]
    float timeDeccelBegins;
    [NonSerialized]
    float timeSinceAccel;
    [NonSerialized]
    float timeSinceDeccel;
    [Header("Camera Parameters")]
    [SerializeField]
    Transform camTransform;
    [SerializeField]
    float mouseSensitivity;
    [Header("Utilities")]
    [SerializeField]
    Item lHand;
    [SerializeField]
    Item rHand;
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
    [SerializeField]
    Image healthBar;
    [SerializeField]
    GroundChecker grChecker;

    private HandSituation hSituation = HandSituation.Free;
    private Animator anim;
    private Scanner scan;
    public Rigidbody rb;
    private GameObject agaDerChild;
    private GameObject agaIzqChild;
    public Item weaponRight;
    public Item weaponLeft;
    public bool wantsThrowLeft = false;
    public bool wantsThrowRight = false;
    public bool wantsPrimaryAction = false;
    public bool wantsJump = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 currentRot = Vector2.zero;
        
        scan = GetComponent<Scanner>();
        StartCoroutine(CheckLastSeenObject());
        anim = GetComponent<Animator>();
        rb.GetComponent<Rigidbody>();
        health = maxHealth;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        GetJumpImput();
        GetCameraInputs();
        MoveCamera();
        GetPrimaryAttackInput();
        GetThrowingInputs();
        HealthBarUpdater();
        scan.SetItems(weaponLeft, weaponRight);
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
        Vector3 speed = movVect * movementSpeed * accelCurve.Evaluate(timeAccelBegins != 0f ? timeSinceAccel : 0);
        rb.AddForce(speed, ForceMode.Force);
        if (speed == Vector3.zero)
        {
            rb.velocity = new Vector3(rb.velocity.x * deccelCurve.Evaluate(timeSinceDeccel), rb.velocity.y, rb.velocity.z * deccelCurve.Evaluate(timeSinceDeccel));
        }
    }

    public void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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
        Rigidbody auxChild = child.GetComponent<Rigidbody>();
        auxChild.isKinematic = true;
        auxChild.velocity = Vector3.zero;
        child.position = parent.position;
        child.localRotation = Quaternion.identity;
        child.GetComponentInChildren<BoxCollider>().enabled = false;
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
        if (lHand.HasHit(out aux, out auxPos))
        {
            Enemy auxEnemy = (Enemy)aux;
            auxEnemy.TakeDamage(strength + lHand.value, auxPos);
        }
        if (rHand.HasHit(out aux, out auxPos))
        {
            Enemy auxEnemy = (Enemy)aux;
            auxEnemy.TakeDamage(strength + rHand.value, auxPos);
        }
    }
    public new void TakeDamage(float damage)
    {
        health -= damage - damage * (protection / 100);
        HealthBarUpdater();
        if (health <= 0)
        {
            Die();
        }
    }

    public new void Die()
    {
        /*rb.isKinematic = true;
        rb.useGravity=false;*/
        Destroy(transform.GetChild(0).GetChild(1).gameObject);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        anim.enabled = true;
        anim.SetBool("IsDead", true);
        this.enabled = false;
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
        BoxCollider boxColl = aux.GetComponentInChildren<BoxCollider>();
        boxColl.enabled = true;
        rgb.velocity = Vector3.zero;
        rgb.isKinematic = false;
        rgb.AddForce(Camera.main.transform.forward * 1, ForceMode.Impulse);
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
            wantsThrowRight = false;
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
            wantsThrowLeft = false;
        }
    }

    public void GetInputs()
    {
        inputVect = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (inputVect != Vector3.zero)
        {
            timeDeccelBegins = 0f;
            if (timeAccelBegins != 0f)
            {
                timeSinceAccel = Time.fixedTime - timeAccelBegins;
                Mathf.Clamp(timeSinceAccel, 0f, 1f);
            }
            else
            {
                timeAccelBegins = Time.fixedTime;
            }
        }
        else
        {
            timeAccelBegins = 0f;
            if (timeDeccelBegins != 0f)
            {
                timeSinceDeccel = Time.fixedTime - timeDeccelBegins;
                Mathf.Clamp(timeSinceDeccel, 0f, 1f);
            }
            else
            {
                timeDeccelBegins = Time.fixedTime;
            }
        }
        movVect = inputVect.x * transform.right + inputVect.z * transform.forward;
        if (Input.GetKeyDown(KeyCode.Space) && grChecker.IsOnGround())
            wantsJump = true;
    }

    public void GetJumpImput()
    {
        if (wantsJump)
            Jump();
        wantsJump = false;
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (handAnimators[0].GetInteger("RHandWeaponType") > 1 && handAnimators[0].GetInteger("RHandWeaponType") < 6)
            {
                handAnimators[0].Play("rHandThrow");
                //wantsThrowRight = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (handAnimators[1].GetInteger("LHandWeaponType") > 1 && handAnimators[1].GetInteger("LHandWeaponType") < 7)
            {
                handAnimators[1].Play("lHandThrow");
                //wantsThrowLeft = true;
            }
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

    }

    public void ClearInputs()
    {
        wantsPrimaryAction = false;
        wantsThrowRight = false;
        wantsThrowLeft = false;
    }

    public void HealthBarUpdater()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
    }
}
