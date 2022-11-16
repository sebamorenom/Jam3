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
    [Header("Animator parameters")]
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Collider rbColl;
    Rigidbody[] rbRagdolls;
    Collider[] rbColliders;
    [SerializeField]
    Collider attackTrigger;
    [SerializeField]
    bool isBoss;
    [SerializeField]
    int numBossAttacks;

    private IEnumerator noise;
    private IEnumerator attackRange;


    public GameObject player;
    private MMF_Player audioPlayer;
    private NavMeshAgent nav;
    private PositionConstraint posConstraint;


    IEnumerator LookForPlayer()
    {
        for (; ; )
        {
            Collider[] aux = Physics.OverlapSphere(transform.position, 20, 1 << LayerMask.NameToLayer("Entity"));
            foreach (Collider coll in aux)
            {
                Debug.Log("Searching");
                if (coll.tag.Contains("Player") && player == null)
                {
                    player = coll.gameObject;
                    audioPlayer.enabled = true;
                    yield break;
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator CheckAttackRange()
    {
        for (; ; )
        {
            if (player != null && Vector3.Distance(transform.position, player.transform.position) < 5f)
            {
                if (isBoss)
                {
                    anim.Play("Attack" + Random.Range(0, numBossAttacks));
                }
                else
                    anim.Play("Attack");
            }
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        rbRagdolls = GetComponentsInChildren<Rigidbody>();
        rbColliders = GetComponentsInChildren<Collider>();
        nav = GetComponentInParent<NavMeshAgent>();
        nav.enabled = false;
        Invoke("EnableNavMeshAgent", 1f);
        ToggleRagdoll(false);
        audioPlayer = GetComponent<MMF_Player>();
        //audioPlayer.enabled = false;
        transform.GetChild(0).TryGetComponent<PositionConstraint>(out posConstraint);
        noise = RandomNoise();
        attackRange = CheckAttackRange();
        StartCoroutine(noise);
        StartCoroutine(LookForPlayer());
        StartCoroutine(attackRange);
        health = maxHealth;
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
        StopCoroutine(noise);
        StopCoroutine(attackRange);
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
        StopCoroutine(attackRange);
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

    public void SwitchAttackTrigger()
    {
        attackTrigger.enabled = !attackTrigger.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Entity") && other.gameObject.tag.Contains("Player"))
        {
            other.GetComponent<Movement>().TakeDamage(strength);
        }
    }

    private void EnableNavMeshAgent()
    {
        nav.enabled = true;
    }
}
