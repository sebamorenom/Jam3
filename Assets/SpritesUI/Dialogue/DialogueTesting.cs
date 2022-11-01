using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Wrappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueDatabase = PixelCrushers.DialogueSystem.DialogueDatabase;

public class DialogueTesting : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Activate();
    }

    public void Activate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DialogueLua.SetVariable("Test", !DialogueLua.GetVariable("Test").asBool);
            Debug.Log(DialogueLua.GetVariable("Test").asString);
            Sequencer.Message("Continue");
        }
    }

}
