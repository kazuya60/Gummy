using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventHandler : MonoBehaviour
{
    private Dictionary<DialogueEventType, Action> dialogueEvents;

    private void Awake()
    {
        dialogueEvents = new Dictionary<DialogueEventType, Action>
        {
            { DialogueEventType.SayHello, SayHello },
            { DialogueEventType.Die, Die }
        };
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

    private void SayHello()
    {
        Debug.Log("Say Helloww");
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
