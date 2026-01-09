using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI charDialogue;
    public Image charSprite;
    public CanvasGroup gameplayUICanvas;


    private DialogueSegment[] segments;
    private int index;
    private bool dialogueActive;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueActive)
            return;

        // Mouse click OR screen tap
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
    segments = dialogue.dialogueSegments;
    index = 0;

    dialogueActive = true;
    dialoguePanel.SetActive(true);

    SetGameplayUIInteractable(false);

    ShowLine();
}


    public void Next()
    {
        index++;

        if (index >= segments.Length)
        {
            EndDialogue();
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

    private void EndDialogue()
{
    dialogueActive = false;
    dialoguePanel.SetActive(false);

    SetGameplayUIInteractable(true);
}

}
