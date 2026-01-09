using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Line_",menuName = "Create New Dialogue")]
public class DialogueSO : ScriptableObject
{
    public DialogueSegment[] dialogueSegments;
    public DialogueChoice[] choices;
    [Header("Automatic continuation (no choices)")]
    public DialogueSO nextDialogue;


}
