using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "JAM/DashAbility")]
public class DashAbility : Ability
{

    public float dashVelocity;
    public override void Activate(GameObject parent)
    {
        PlayerMovementRigidbody movement = parent.GetComponent<PlayerMovementRigidbody>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        rigidbody.velocity = new Vector3(movement.horizontalInput, movement.transform.position.y, movement.verticalInput).normalized * dashVelocity;
    }

    public override void BeginCooldown(GameObject parent)
    {
        PlayerMovementRigidbody movement = parent.GetComponent<PlayerMovementRigidbody>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        rigidbody.velocity = new Vector3(movement.horizontalInput, movement.transform.position.y, movement.verticalInput).normalized * movement.moveSpeed;
    }
}

