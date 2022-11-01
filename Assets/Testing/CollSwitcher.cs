using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    private Collider[] colliders;

    public void SetCollider(Collider[] newColliders)
    {
        colliders = newColliders;
    }

    public void Switch()
    {
        foreach (Collider coll in colliders)
        {
            coll.enabled = !coll.enabled;
        }
    }
}
