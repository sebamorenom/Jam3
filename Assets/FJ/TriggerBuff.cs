using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBuff : MonoBehaviour
{
    public string[] stat;
    public float[] buff;
    public float time;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("He entrado");
            StartCoroutine(other.gameObject.GetComponent<Movement>().BuffDebuff(stat, buff, time));
        }
    }
}
