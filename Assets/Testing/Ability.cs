using JetBrains.Annotations;
using Opsive.UltimateInventorySystem.Demo.CharacterControl;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum AbilityType { Melee, RangedProjectile, RangedSpawn, Buff };

public class Ability : MonoBehaviour
{
    [SerializeField]
    public AbilityType type;
    [SerializeField]
    public float damage;
    [SerializeField]
    public float manaCost;
    [SerializeField]
    GameObject toSpawn;
    [SerializeField]
    Transform spawnFrom;
    [SerializeField]
    string[] statsToBuff;
    [SerializeField]
    float[] buffForStats;



    private bool userIsPlayer;
    private Transform player;

    void Start()
    {
        userIsPlayer = GetComponentInParent<Movement>()!=null?true:false;
    }


    public void PrimaryAction(Animator anim)
    {
        switch (type)
        {
            case AbilityType.Melee:
                anim.SetTrigger("Primary");
                break;
            case AbilityType.RangedProjectile:
                Vector3 dir = Vector3.zero;
                if (userIsPlayer)
                {
                    dir = Camera.main.transform.forward;
                }
                else
                {
                    dir = (player.position - transform.position).normalized;
                }
                Instantiate(toSpawn).GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward);
                break;
            case AbilityType.RangedSpawn:
                RaycastHit ray;
                Physics.Raycast(spawnFrom.position, Camera.main.transform.forward, out ray, 12, 1 << LayerMask.NameToLayer("Level"));
                Instantiate(toSpawn, ray.point, Quaternion.identity);
                break;
            case AbilityType.Buff:
                //GetComponentInParent<Humanoid>().Buff(statsToBuff,buffForStats);
                break;
                
        }
    }

    public void SecondaryAction()
    {

    }

    public void PlayerFound(Transform _player)
    {
        player = _player;
    }

}
