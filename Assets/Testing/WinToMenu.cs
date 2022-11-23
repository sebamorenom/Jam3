using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinToMenu : MonoBehaviour
{
    [SerializeField]
    Enemy finalBoss;
    [SerializeField]
    Animator anim;
    [SerializeField]
    ChangeScene sceneCh;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (finalBoss.health <= 0)
        {
            anim.Play("LightBoss");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (finalBoss.health <= 0)
        {
            sceneCh.Loadscene("UI");
        }
    }
}
