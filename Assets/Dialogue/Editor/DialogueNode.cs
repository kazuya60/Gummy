using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class DialogueNode : Node
{
    public string DialogueText;
    public string GUID;
    public bool EntryPoint = false;
}