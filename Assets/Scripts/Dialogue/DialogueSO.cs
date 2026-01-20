using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Line_", menuName = "Create New Dialogue")]
public class DialogueSO : ScriptableObject
{
    public DialogueSegment[] dialogueSegments;

    [Header("Interact Choice")]
public DialogueSO interactDialogue;
public DialogueEventType interactEvent;

[Header("Reject Choice")]
public DialogueSO rejectDialogue;
public DialogueEventType rejectEvent;


    [Header("Automatic continuation (no choices)")]
    public DialogueSO nextDialogue;

    public DialogueEventType startEvent;
    public DialogueEventType endEvent;
}

