using DunGen.Demo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class Movement : MonoBehaviour
{
    Vector3 movVect;
    Vector2 camMov;
    Vector2 camRot;
    [SerializeField]
    Transform camTransform;
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float mouseSensitivity;
    [SerializeField]
    bool destroy;
    [SerializeField]
    GameObject agaIzq;
    [SerializeField]
    GameObject agaDer;
    [SerializeField]
    Item currWeap;

    private Animator anim;
    private Scanner scan;
    private Rigidbody rb;
    public bool wantsThrow;
    public bool wantsPrimaryAction;

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
        movVect = new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime);
        camMov = new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity, Input.GetAxis("Mouse Y") * mouseSensitivity);
        if (Input.GetKeyDown(KeyCode.Mouse3))
        {
            wantsThrow = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            wantsPrimaryAction = true;
        }
        MoveCamera();

    }

    private void FixedUpdate()
    {
        if (agaDer.transform.childCount != 0 && wantsThrow)
        {
            Throw(agaDer.transform);
            wantsThrow = false;
        }
        if (agaDer.transform.childCount != 0 && wantsThrow)
        {
            Throw(agaIzq.transform);
            wantsThrow = false;
        }
        wantsThrow = false;
        if (wantsPrimaryAction && currWeap != null)
        {
            currWeap.PrimaryAction(anim);
            wantsPrimaryAction = false;
        }
        MoveCharacter();
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
        Vector3 aux = movVect.z * transform.forward + movVect.x * transform.right;
        Debug.Log(aux);
        transform.Translate(aux.x, aux.y, aux.z);
    }

    public void HacePum()
    {
        Destroy(this.gameObject);
    }

    IEnumerator CheckLastSeenObject()
    {
        GameObject aux = null;
        for (; ; )
        {
            scan.GetLastSeenItem(out aux);
            if (aux != null)
            {
                if (agaDer.transform.childCount == 0)
                {
                    Parent(agaDer.transform, aux.transform);
                    break;
                }
                if (agaIzq.transform.childCount == 0)
                {
                    Parent(agaIzq.transform, aux.transform);
                    break;
                }
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void Parent(Transform parent, Transform child)
    {
        child.SetParent(parent);
        child.position = parent.position;
        child.transform.right = -Camera.main.transform.forward;
        child.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Throw(Transform parentHand)
    {
        Transform aux = parentHand.GetChild(0);
        aux.parent = null;
        Rigidbody rgb = aux.GetComponent<Rigidbody>();
        rgb.isKinematic = false;
        rgb.AddForce(Camera.main.transform.forward * 9, ForceMode.Impulse);
        rgb.AddForceAtPosition(Camera.main.transform.forward * 6f, aux.position + 3f * aux.up, ForceMode.Impulse);

    }
}
