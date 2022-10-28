using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    public Transform playerTransform;
    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        Invoke("ActivateNav", 1f);

    }

    // Update is called once per frame
    void Update()
    {
        HeadTo();
    }

    public void HeadTo()
    {
        nav?.SetDestination(playerTransform.transform.position);
    }

    public void ActivateNav()
    {
        nav.enabled = true;
    }
}
