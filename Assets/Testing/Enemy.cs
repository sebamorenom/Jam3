using PixelCrushers.DialogueSystem.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    // Start is called before the first frame update

    private Animator anim;
    [Header("Animator paramters")]
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Collider rbColl;
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

    }

    public void HeadTo()
    {
        if (nav.enabled)
        {
            if (player != null)
                nav.SetDestination(player.transform.position);
        }
        transform.LookAt(nav.steeringTarget);
    }

    public void TakeDamage(float damage, Vector3 collisionPoint)
    {
        health -= damage - damage * (protection / 100);
        if (health <= 0)
        {
            Die(collisionPoint, damage);
        }
    }


    public void Die(Vector3 collPoint, float explosionForce)
    {
        Debug.Log("Muerto");
        ToggleRagdoll(true);
        nav.enabled = false;
        foreach (Rigidbody rb in rbRagdolls)
        {
            rb.AddExplosionForce(explosionForce, collPoint, 1f, 0f, ForceMode.Impulse);
        }
    }

    public new void Die()
    {
        Debug.Log("Muerto");
        ToggleRagdoll(true);
        nav.enabled = false;
    }

    public void ToggleRagdoll(bool state)
    {
        anim.enabled = !state;
        foreach (Rigidbody aux in rbRagdolls)
        {
            aux.isKinematic = !state;
        }
        rb.isKinematic = state;
        foreach (Collider aux in rbColliders)
        {
            aux.enabled = state;
        }
        rbColl.enabled = !state;
    }
    public void ToAnimator()
    {
        anim.SetFloat("Speed", Mathf.Abs(nav.velocity.magnitude));
    }
}
