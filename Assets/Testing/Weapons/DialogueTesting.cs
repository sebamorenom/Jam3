using PixelCrushers;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Wrappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueDatabase = PixelCrushers.DialogueSystem.DialogueDatabase;
using DialogueSystemTrigger = PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger;

public class DialogueTesting : MonoBehaviour
{
    PixelCrushers.DialogueSystem.ConversationController conver;

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
            Sequencer.Message("Continue");
        }
    }

}
