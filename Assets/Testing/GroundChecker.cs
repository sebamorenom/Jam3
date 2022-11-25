using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    bool onGround;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 2)
            onGround = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer != 2)
            onGround = false;
    }

    public bool IsOnGround()
    {
        return onGround;
    }
}
