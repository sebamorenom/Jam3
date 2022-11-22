using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUsable : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    Transform spawningPos;
    [SerializeField]
    MerchantStash merStash;
    [SerializeField]
    public bool isMimic;
    [SerializeField]
    public GameObject mimicPrefab;

    AudioSource audioSrc;
    // Start is called before the first frame update
    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }
    public void Open()
    {
        if (!isMimic)
        {
            anim.Play("Open");
            audioSrc.Play();
        }

        else
            Instantiate(mimicPrefab);

    }

    public void SpawnObject()
    {
        GameObject aux = merStash.GetRandomItems(1)[0];
        aux.transform.parent = spawningPos.transform;
        aux.transform.position = spawningPos.transform.position;
        aux.SetActive(true);
    }
}
