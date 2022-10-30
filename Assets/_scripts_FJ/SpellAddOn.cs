using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAddOn : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponentInChildren<ParticleSystem>().Play();
        Destroy(gameObject, 0.5f);
    }
}
