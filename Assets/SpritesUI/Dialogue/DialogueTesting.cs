using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Wrappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueDatabase = PixelCrushers.DialogueSystem.DialogueDatabase;

public class DialogueTesting : MonoBehaviour
{
    [SerializeField]
    string variableName;
    [SerializeField]
    TMPro.TextMeshProUGUI tmPro;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        tmPro.SetText(DialogueLua.GetVariable(variableName).asString);
    }
}
