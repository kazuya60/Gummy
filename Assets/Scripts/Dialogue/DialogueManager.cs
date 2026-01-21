using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public GameObject introPanel;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI charDialogue;
    public Image charSprite;
    public CanvasGroup gameplayUICanvas;

    [Header("Interact / Reject UI")]
    public GameObject decisionPanel;
    public Button interactButton;
    public Button rejectButton;


    private DialogueSO currentDialogue;
    private DialogueSegment[] segments;
    private int index;
    private bool dialogueActive;

    public DialogueEventHandler dialogueEventHandler;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        decisionPanel.SetActive(false);
        introPanel.SetActive(true);
    }

    private void Update()
    {
        if (!dialogueActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Next();
        }
    }

    private void SetGameplayUIInteractable(bool value)
    {
        gameplayUICanvas.interactable = value;
        gameplayUICanvas.blocksRaycasts = value;
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        currentDialogue = dialogue;
        segments = dialogue.dialogueSegments;
        index = 0;

        dialogueActive = true;
        dialoguePanel.SetActive(true);
        StartEventStarter();

        SetGameplayUIInteractable(false);

        ShowLine();
    }

    public void Next()
    {
        index++;

        if (index >= segments.Length)
        {
            ShowEndChoicesOrEnd();
            return;
        }

        ShowLine();
    }

    private void ShowLine()
    {
        var line = segments[index];

        charName.text = line.character.CharacterName;
        charDialogue.text = line.ActorDialogue;

        if (line.character.CharacterSprite != null)
        {
            charSprite.gameObject.SetActive(true);
            charSprite.sprite = line.character.CharacterSprite;
        }
        else
        {
            charSprite.gameObject.SetActive(false);
        }
    }


    private void ShowEndChoicesOrEnd()
    {
        dialogueActive = false;

        // Interact / Reject takes priority
        if (currentDialogue.interactDialogue != null ||
            currentDialogue.rejectDialogue != null)
        {
            ShowDecisionButtons();
            return;
        }

        // Auto-continue
        if (currentDialogue.nextDialogue != null)
        {
            StartDialogue(currentDialogue.nextDialogue);
            return;
        }

        EndDialogue();
    }

    private void ShowDecisionButtons()
    {
        decisionPanel.SetActive(true);

        interactButton.gameObject.SetActive(
            currentDialogue.interactDialogue != null ||
            currentDialogue.interactEvent != DialogueEventType.None
        );

        rejectButton.gameObject.SetActive(
            currentDialogue.rejectDialogue != null ||
            currentDialogue.rejectEvent != DialogueEventType.None
        );

        interactButton.onClick.RemoveAllListeners();
rejectButton.onClick.RemoveAllListeners();

interactButton.onClick.AddListener(ForceInteract);
rejectButton.onClick.AddListener(ForceReject);

    }



    private void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
        decisionPanel.SetActive(false);

        EndEventStarter();

        SetGameplayUIInteractable(true);
    }

    void StartEventStarter()
    {
        dialogueEventHandler.TriggerEvent(currentDialogue.startEvent);
    }

    void EndEventStarter()
    {
        dialogueEventHandler.TriggerEvent(currentDialogue.endEvent);
    }

    public void ForceReject()
    {
        // Safety: only reject when a decision is actually active
        if (!decisionPanel.activeSelf)
            return;

        decisionPanel.SetActive(false);

        // Fire reject event
        if (currentDialogue.rejectEvent != DialogueEventType.None)
        {
            dialogueEventHandler.TriggerEvent(currentDialogue.rejectEvent);
        }

        // Continue reject dialogue if assigned
        if (currentDialogue.rejectDialogue != null)
        {
            StartDialogue(currentDialogue.rejectDialogue);
        }
        else
        {
            EndDialogue();
        }
    }

    public void ForceInteract()
    {
        // Safety: only interact when a decision is actually active
        if (!decisionPanel.activeSelf)
            return;

        decisionPanel.SetActive(false);

        // Fire interact event
        if (currentDialogue.interactEvent != DialogueEventType.None)
        {
            dialogueEventHandler.TriggerEvent(currentDialogue.interactEvent);
        }

        // Continue interact dialogue if assigned
        if (currentDialogue.interactDialogue != null)
        {
            StartDialogue(currentDialogue.interactDialogue);
        }
        else
        {
            EndDialogue();
        }
    }




}
