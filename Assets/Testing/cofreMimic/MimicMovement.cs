using Rewired.Integration.UnityUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicMovement : MonoBehaviour
{
    [SerializeField]
    Transform player;


    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
    }

    public void AttackJump()
    {
        rb.AddForce(transform.forward * 10, ForceMode.Impulse);
    }
}
