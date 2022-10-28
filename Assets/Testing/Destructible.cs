using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject brokenModel;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject aux = Instantiate(brokenModel, transform.position, transform.rotation);
            foreach (Rigidbody ux in aux.GetComponentsInChildren<Rigidbody>())
            {
                ux.AddExplosionForce(300, transform.position, 2);
            }
            Destroy(this.gameObject);
        }
    }
}
