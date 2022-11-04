using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using PixelCrushers.DialogueSystem.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

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
    private IEnumerator noise;


    public GameObject player;
    private MMF_Player audioPlayer;
    private NavMeshAgent nav;
    private PositionConstraint posConstraint;
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        rbRagdolls = GetComponentsInChildren<Rigidbody>();
        rbColliders = GetComponentsInChildren<Collider>();
        nav = GetComponentInParent<NavMeshAgent>();
        ToggleRagdoll(false);
        audioPlayer = GetComponent<MMF_Player>();
        transform.GetChild(0).TryGetComponent<PositionConstraint>(out posConstraint);
        noise = RandomNoise();
        StartCoroutine(noise);

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
        audioPlayer.GetFeedbackOfType<MMF_MMSoundManagerSound>("Hurt").Play(transform.position);
        health -= damage - damage * (protection / 100);
        if (health <= 0)
        {
            Die(collisionPoint, damage);
        }
    }


    public void Die(Vector3 collPoint, float explosionForce)
    {
        if (posConstraint != null)
            posConstraint.enabled = false;
        audioPlayer.GetFeedbackOfType<MMF_MMSoundManagerSound>("Dying").Play(transform.position);
        ToggleRagdoll(true);
        Debug.Log("Muere");
        StopCoroutine(noise);
        nav.enabled = false;
        foreach (Rigidbody rb in rbRagdolls)
        {
            rb.AddExplosionForce(explosionForce, collPoint, 1f, 0f, ForceMode.Impulse);
        }
    }

    public new void Die()
    {
        if (posConstraint != null)
            posConstraint.enabled = false;
        audioPlayer.GetFeedbackOfType<MMF_MMSoundManagerSound>("Dying").Play(transform.position);
        StopCoroutine(noise);
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

    public void PlayWalkingSound()
    {
        audioPlayer.GetFeedbackOfType<MMF_MMSoundManagerSound>("Walking").Play(transform.position);
    }

    IEnumerator RandomNoise()
    {
        for (; ; )
        {
            if (Random.value < 0.3)
            {
                audioPlayer.GetFeedbackOfType<MMF_MMSoundManagerSound>("Noise").Play(transform.position);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
