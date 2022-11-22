using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingZone : MonoBehaviour
{
    // Start is called before the first frame update
    bool alreadyHealed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player") && !alreadyHealed)
        {
            Movement aux = other.gameObject.GetComponent<Movement>();
            aux.Heal(aux.maxHealth / 2);
            alreadyHealed = true;
        }
    }
}
