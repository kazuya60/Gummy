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

    public DialogueSO veeDialogue;
    public void VeeDialogue()
    {
        DialogueManager.Instance.StartDialogue(veeDialogue);
    }



    
    
    // public void GoOnline()
    // {
    //     DialogueManager.Instance.ForceReject();
    // }

    // public void DozeOff()
    // {
    //     DialogueManager.Instance.ForceReject();
    // }

    // public void DoomScroll()
    // {
    //     DialogueManager.Instance.ForceReject();
    // }
}
