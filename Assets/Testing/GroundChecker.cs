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
    private void OnTriggerEnter(Collider other)
    {
        onGround = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        onGround = false;
    }

    public bool IsOnGround()
    {
        return onGround;
    }
}
