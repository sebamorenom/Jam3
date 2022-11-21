using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardTrigger : MonoBehaviour
{
    private Animator wizardTrigger;
    // Start is called before the first frame update
    void Start()
    {
        wizardTrigger = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            wizardTrigger.SetBool("Activate", true);
        }
    }
}
