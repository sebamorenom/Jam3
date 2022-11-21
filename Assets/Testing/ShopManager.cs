using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public static class ShopManager
{
    static List<Collider> shopsColliders = new List<Collider>();
    static bool isWitchDead = false;

    // Start is called before the first frame update
    public static void AddToList(Collider coll)
    {
        shopsColliders.Add(coll);
    }
    public static void DestroyWitch()
    {
        if (!isWitchDead)
        {
            DialogueLua.SetVariable("WitchDie", true);
            isWitchDead = true;
            foreach (Collider coll in shopsColliders)
            {
                Animator anim = coll.GetComponent<Animator>();
                anim.SetBool("Activate", true);
            }
        }

    }
}
