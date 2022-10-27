using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "JAM/DashAbility")]
public class DashAbility : Ability
{

    public float dashVelocity;
    public override void Activate(GameObject parent)
    {
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();

        movement.velocity = movement.velocity.normalized * dashVelocity;
    }
}
