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
    public GameObject characterSpritesParent;
    public GameObject TeacherCharacter;
    public GameObject AprilCharacter;

    [Header("Background Controller")]
    public BackgroundController backgroundController;
    private Dictionary<DialogueEventType, Action> dialogueEvents;
    private Dictionary<GlobalActionType, Action> globalActions;

    public DialogueSO AprilDialogue;
    public DialogueSO BelladonnaInteractDialogue;
    public DialogueSO BelladonnaRejectDialogue;
    public DialogueSO VeeDialogue;
    public ChoiceType choiceType;


    private void Awake()
    {
        globalActions = new Dictionary<GlobalActionType, Action>
    {
        { GlobalActionType.DoomScroll, OnDoomScroll },
        { GlobalActionType.GoOnline, OnGoOnline },
        { GlobalActionType.DozeOff, OnDozeOff }
    };


        dialogueEvents = new Dictionary<DialogueEventType, Action>
    {
        { DialogueEventType.StartSpotDifference, StartSpotDifference },
        { DialogueEventType.EndSpotDifference, EndSpotDifference },
        { DialogueEventType.DifferenceSuccess, FindDifferenceSuccess },
        { DialogueEventType.DifferenceFailure, FindDifferenceFailure },
        { DialogueEventType.StartAprilDialogue, StartAprilDialogue },
        { DialogueEventType.StartBelladonnaDialogue, StartBelladonnaDialogue },
        { DialogueEventType.SetChoiceBella1, () => choiceType = ChoiceType.BellaInteract },
        { DialogueEventType.SetChoiceBella2, () => choiceType = ChoiceType.BellaReject },
        { DialogueEventType.SetChoiceVee, () => choiceType = ChoiceType.Vee },

        { DialogueEventType.ActivateCharacterSprites, () => SetCharacterSpritesActive(true) },
        { DialogueEventType.DeactivateCharacterSprites, () => SetCharacterSpritesActive(false) },
        { DialogueEventType.DisableBothAprilAndTeacher, DisableBothAprilAndTeacher },
        { DialogueEventType.ActivateNavigation, () => RoomManager.Instance.enableNavigation = true },
        { DialogueEventType.DeactivateNavigation, () => RoomManager.Instance.enableNavigation = false }


        
    };

        RefillPool();
    }

    private void StartAprilDialogue()
    {
        DialogueManager.Instance.StartDialogue(AprilDialogue);
    }

    private void StartBelladonnaDialogue()
    {
        if (choiceType == ChoiceType.BellaInteract)
            DialogueManager.Instance.StartDialogue(BelladonnaInteractDialogue);
        else if (choiceType == ChoiceType.Vee)
        {
            DialogueManager.Instance.StartDialogue(VeeDialogue);
        }
        else
            DialogueManager.Instance.StartDialogue(BelladonnaRejectDialogue);
    }

    

    void OnDoomScroll()
    {
        StatManager.Instance.AddGummy(10);
        StatManager.Instance.AddSocialAnxiety(-10);
        Debug.Log("Doom Scrolled");
    }

    void OnGoOnline()
    {
        StatManager.Instance.AddGummy(7);
        StatManager.Instance.AddSocialAnxiety(-7);
        Debug.Log("Went Online");
    }

    void OnDozeOff()
    {
        StatManager.Instance.AddGummy(5);
        StatManager.Instance.AddSocialAnxiety(-5);
        Debug.Log("Dozed Off");
    }

    public void TriggerGlobalAction(GlobalActionType action)
    {
        if (action == GlobalActionType.None)
            return;

        if (globalActions.TryGetValue(action, out var a))
            a.Invoke();
    }



    private void SetCharacterSpritesActive(bool active)
    {
        characterSpritesParent.SetActive(active);
    }

    public void DisableCharacter(GameObject character)
{
    character.SetActive(false);
}


    public void DisableBothAprilAndTeacher()
    {
        DisableCharacter(TeacherCharacter);
        DisableCharacter(AprilCharacter);
        RoomManager.Instance.enableNavigation = true;
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

        if (src != null && src.failTask != null)
    TaskManager.Instance.FailTask(src.failTask);


        if (src != null)
        {
            StatManager.Instance.ApplyDelta(src.loseStats);

            if (src.differenceLoseDialogue != null)
                DialogueManager.Instance.StartDialogue(src.differenceLoseDialogue);
        }
    }



    public void FindDifferenceSuccess()
    {
        EndSpotDifference();

        var src = minigameSourceDialogue;

        if (src != null && src.completeTask != null)
    TaskManager.Instance.CompleteTask(src.completeTask);


        if (src != null)
        {
            StatManager.Instance.ApplyDelta(src.winStats);

            if (src.differenceWinDialogue != null)
                DialogueManager.Instance.StartDialogue(src.differenceWinDialogue);
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
