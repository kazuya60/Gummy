using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueLine",menuName = "Create New Dialogue")]
public class DialogueSO : ScriptableObject
{
    public DialogueSegment[] dialogueSegments;
}
