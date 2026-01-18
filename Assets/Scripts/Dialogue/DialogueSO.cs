using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Line_", menuName = "Create New Dialogue")]
public class DialogueSO : ScriptableObject
{
    public DialogueSegment[] dialogueSegments;

    [Header("End Choices")]
    public DialogueSO interactDialogue;
    public DialogueSO rejectDialogue;

    [Header("Automatic continuation (no choices)")]
    public DialogueSO nextDialogue;

    public DialogueEventType startEvent;
    public DialogueEventType endEvent;
}

