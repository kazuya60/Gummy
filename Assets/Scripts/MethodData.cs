using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodData : MonoBehaviour
{
    public DialogueSO gummyStarterDialogue;
    public void GummyDialogueStarter()
    {
        DialogueManager.Instance.StartDialogue(gummyStarterDialogue);
    }
}
