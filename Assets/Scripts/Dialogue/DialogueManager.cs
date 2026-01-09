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

    [Header("Choices UI")]
    public GameObject choicesPanel;
    public Button choiceButtonPrefab;

    private DialogueSO currentDialogue;
    private DialogueSegment[] segments;
    private int index;
    private bool dialogueActive;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);
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

        ClearChoices();
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
        charSprite.sprite = line.character.CharacterSprite;
        charDialogue.text = line.ActorDialogue;
    }

    private void ShowEndChoicesOrEnd()
{
    dialogueActive = false;

    // 1. Choices take priority
    if (currentDialogue.choices != null &&
        currentDialogue.choices.Length > 0)
    {
        ShowChoices(currentDialogue.choices);
        return;
    }

    // 2. No choices → auto-continue if linked
    if (currentDialogue.nextDialogue != null)
    {
        StartDialogue(currentDialogue.nextDialogue);
        return;
    }

    // 3. Nothing else → end dialogue
    EndDialogue();
}


    private void ShowChoices(DialogueChoice[] choices)
    {
        choicesPanel.SetActive(true);

        foreach (var choice in choices)
        {
            Button btn = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

            btn.onClick.AddListener(() =>
            {
                ClearChoices();
                StartDialogue(choice.nextDialogue);
            });
        }
    }

    private void ClearChoices()
    {
        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        choicesPanel.SetActive(false);
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);

        ClearChoices();
        SetGameplayUIInteractable(true);
    }
}
