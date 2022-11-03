using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "JAM/Spells")]
public class Spells : Ability
{
    public GameObject prefab;
    public Transform shootingPoint;
    public Transform cam;
    public float throwForce;
    public float throwUpdwardForce;
    Rigidbody projectileRb;
    public float damage;

    public override void Activate(GameObject parent)
    {
        shootingPoint = parent.transform.GetChild(4);
        cam = parent.GetComponentInChildren<Camera>().transform;
        GameObject projectile  = Instantiate(prefab, shootingPoint.transform.position, Quaternion.identity);
        projectileRb = projectile.GetComponent<Rigidbody>();
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - shootingPoint.transform.position).normalized;
        }

        

        Vector3 forceToAdd = forceDirection * throwForce + Vector3.up * throwUpdwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
    }
}
