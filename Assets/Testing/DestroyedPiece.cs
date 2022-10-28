using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedPiece : MonoBehaviour
{
    // Start is called before the first frame update
    bool pickable;
    [SerializeField]
    float timeBeforeDestruction;
    [SerializeField]
    float timeBeforeBlinking;
    MeshRenderer meshRend;
    void Start()
    {
        meshRend = GetComponentInChildren<MeshRenderer>();
        StartCoroutine(Delay(timeBeforeBlinking));
        Destroy(gameObject, timeBeforeDestruction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Blink()
    {
        for (; ;)
        {
            meshRend.enabled = !meshRend.enabled;
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator Delay(float blinkingDelay)
    {
        yield return new WaitForSeconds(blinkingDelay);
        StartCoroutine(Blink());
    }
}
