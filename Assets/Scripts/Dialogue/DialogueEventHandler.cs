using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventHandler : MonoBehaviour
{
    [Header("Minigames")]
public GameObject[] spotDifferencePrefabs;

private GameObject activeSpotDifference;
private List<int> unusedIndices = new List<int>();



    public GameObject dialogueCanvas;
    private DialogueSO minigameSourceDialogue;

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
        { DialogueEventType.DifferenceFailure, () => FindDifferenceFailure() }
    };

    RefillPool();
}

private void RefillPool()
{
    unusedIndices.Clear();
    for (int i = 0; i < spotDifferencePrefabs.Length; i++)
        unusedIndices.Add(i);
}

    public void FindDifferenceFailure()
{
    EndSpotDifference();

    var src = minigameSourceDialogue;

    if (src != null && src.differenceLoseDialogue != null)
        DialogueManager.Instance.StartDialogue(src.differenceLoseDialogue);
}


    public void FindDifferenceSuccess()
{
    EndSpotDifference();

    var src = minigameSourceDialogue;

    if (src != null && src.differenceWinDialogue != null)
        DialogueManager.Instance.StartDialogue(src.differenceWinDialogue);
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
    minigameSourceDialogue = DialogueManager.Instance.CurrentDialogue;
    
    if (activeSpotDifference != null)
        Destroy(activeSpotDifference);

    if (unusedIndices.Count == 0)
        RefillPool();

    int pick = UnityEngine.Random.Range(0, unusedIndices.Count);
    int prefabIndex = unusedIndices[pick];
    unusedIndices.RemoveAt(pick);

    activeSpotDifference = Instantiate(spotDifferencePrefabs[prefabIndex]);
    activeSpotDifference.SetActive(true);

    dialogueCanvas.SetActive(false);
}



private void EndSpotDifference()
{
    if (activeSpotDifference != null)
        Destroy(activeSpotDifference);

    activeSpotDifference = null;
    dialogueCanvas.SetActive(true);

    Debug.Log("Spot the Difference ended");
}





    
}
