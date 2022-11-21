using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Animator groupAnim;
    [SerializeField]
    Animator wizardAnim;


    public void SwitchAnimators()
    {
        groupAnim.enabled = false;
        wizardAnim.enabled = true;
    }
}
