using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Currency", menuName = "ScriptableObjects/Currency", order = 1)]
public class Currency: ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject[] notes;
    [SerializeField]
    public float[] notesValues;

    // Update is called once per frame
    
    public void Exchange(Vector3 position, float valueToExchange)
    {
        for(int i = notesValues.Length-1; i >= 0; i--)
        {
            while (valueToExchange >= notesValues[i])
            {
                valueToExchange -= notesValues[i];
                Instantiate(notes[i], position, Quaternion.identity);
            }
        }
    }  
}
