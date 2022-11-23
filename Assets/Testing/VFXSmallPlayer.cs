using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXSmallPlayer : MonoBehaviour
{
    [SerializeField]
    VisualEffect vfx;
    [SerializeField]
    AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayVFX()
    {
        vfx.Play();
        audioSrc.Play();
    }
}
