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

    [Header("Phone Choice (Shared Dialogue)")]
    public DialogueSO phoneDialogue;

    public DialogueEventType doomScrollEvent;
    public DialogueEventType dozeOffEvent;
    public DialogueEventType goOnlineEvent;

    [Header("Minigame Outcomes")]
    public DialogueSO differenceWinDialogue;
    public DialogueSO differenceLoseDialogue;



    [Header("Background")]
    public BackgroundSO startBackground;
    public BackgroundSO endBackground;





}

