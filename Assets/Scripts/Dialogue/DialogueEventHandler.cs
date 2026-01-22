using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventHandler : MonoBehaviour
{
    [Header("Minigames")]
    public GameObject spotDifferenceCanvas;
    public GameObject dialogueCanvas;
    public DialogueSO successDialogue;
    public DialogueSO failureDialogue;
    [Header("Background Controller")]
    public BackgroundController backgroundController;
    private Dictionary<DialogueEventType, Action> dialogueEvents;

    private void Awake()
    {
        dialogueEvents = new Dictionary<DialogueEventType, Action>
        {
            { DialogueEventType.StartSpotDifference, StartSpotDifference },
            { DialogueEventType.EndSpotDifference, EndSpotDifference },
            { DialogueEventType.DifferenceSuccess, () => FindDifferenceSuccess() },
            { DialogueEventType.DifferenceFailure, () => FindDifferenceFailure() },

            
            
        };
    }

    public void FindDifferenceFailure()
    {
        if (failureDialogue != null)
        {
            EndSpotDifference();
            DialogueManager.Instance.StartDialogue(failureDialogue);
        }
    }

    public void FindDifferenceSuccess()
    {
        if (successDialogue != null)
        {
            EndSpotDifference();
            DialogueManager.Instance.StartDialogue(successDialogue);
        }
    }

    public void TriggerEvent(DialogueEventType eventType)
    {
        if (eventType == DialogueEventType.None)
            return;

        if (dialogueEvents.TryGetValue(eventType, out var action))
        {
            action.Invoke();
        }
        else
        {
            Debug.LogWarning($"No dialogue event registered for {eventType}");
        }
    }


    public void StartSpotDifference()
{
    spotDifferenceCanvas.SetActive(true);
    dialogueCanvas.SetActive(false);
    Debug.Log("Spot the Difference started");
}

private void EndSpotDifference()
{
    spotDifferenceCanvas.SetActive(false);
    dialogueCanvas.SetActive(true);
    Debug.Log("Spot the Difference ended");
}




    
}
