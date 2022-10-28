using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdollizer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Animator anim;
    [SerializeField]
    Rigidbody rbAnimator;
    [SerializeField]
    Collider rbAnimColl;
    Rigidbody[] rbRagdolls;
    Collider[] rbColliders;


    public GameObject player;
    private NavMeshAgent nav;
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        rbRagdolls = GetComponentsInChildren<Rigidbody>();
        rbColliders = GetComponentsInChildren<Collider>();
        nav = GetComponentInParent<NavMeshAgent>();
        ToggleRagdoll(false);

    }

    // Update is called once per frame
    void Update()
    {
        HeadTo();
        if (anim.enabled)
        {
            ToAnimator();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("HitColliders"))
        {
            Die(collision.contacts[0].point);
        }
    }

    public void HeadTo()
    {
        if (nav.enabled)
        {
            nav.SetDestination(player.transform.position);
        }
        transform.LookAt(nav.velocity);
    }

    private void Die(Vector3 collPoint)
    {
        ToggleRagdoll(true);
        nav.enabled = false;
        foreach (Rigidbody rb in rbRagdolls)
        {
            rb.AddExplosionForce(100f, collPoint, 1f, 0f, ForceMode.Impulse);
        }
    }

    public void ToggleRagdoll(bool state)
    {
        anim.enabled = !state;
        foreach (Rigidbody aux in rbRagdolls)
        {
            aux.isKinematic = !state;
        }
        rbAnimator.isKinematic = state;
        foreach (Collider aux in rbColliders)
        {
            aux.enabled = state;
        }
        rbAnimColl.enabled = !state;
    }
    public void ToAnimator()
    {
        anim.SetFloat("Speed", Mathf.Abs(nav.velocity.magnitude));
    }
}
