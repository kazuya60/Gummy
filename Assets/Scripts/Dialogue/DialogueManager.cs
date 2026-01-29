using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [Header("Line Finished Indicator")]
[SerializeField] GameObject lineFinishedImage;


    [Header("Typewriter")]
[SerializeField] float typeSpeed = 0.03f;

Coroutine typingRoutine;
bool isTyping;
string fullLine;

[Header("Typewriter Volume")]
[SerializeField] float typingVolume = 0.25f;

[Header("Dialogue Ducking")]
[SerializeField] AudioSource musicSource;
[SerializeField] float duckVolume = 0.5f;
[SerializeField] float duckSpeed = 6f;

float originalMusicVolume;



[Header("Typewriter Audio")]
[SerializeField] AudioSource typeAudioSource;
[SerializeField] AudioClip[] typingClips;
[SerializeField] float soundCooldown = 0.04f;

float lastSoundTime;

[Header("Typewriter Punctuation")]
[SerializeField] float commaPause = 0.15f;
[SerializeField] float sentencePause = 0.35f;

[Header("Typewriter Pitch")]
[SerializeField] Vector2 pitchRange = new Vector2(0.9f, 1.1f);




    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public GameObject introPanel;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI charDialogue;
    public GameObject nameplateRoot;

    public Image charSprite;
    public CanvasGroup gameplayUICanvas;
    public BackgroundController backgroundController;

    [Header("Interact / Reject UI")]
    public GameObject decisionPanel;
    public Button interactButton;
    public Button rejectButton;

    [Header("Phone Buttons")]
    public Button doomScrollButton;
    public Button dozeOffButton;
    public Button goOnlineButton;



    private DialogueSO currentDialogue;
    public DialogueSO CurrentDialogue => currentDialogue;

    private bool phoneChoicesActive;

    private DialogueSegment[] segments;
    private int index;
    private bool dialogueActive;

    public DialogueEventHandler dialogueEventHandler;

    public bool isNavigationEnabled = false;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        decisionPanel.SetActive(false);
        introPanel.SetActive(true);

        doomScrollButton.onClick.AddListener(OnDoomScroll);
        dozeOffButton.onClick.AddListener(OnDozeOff);
        goOnlineButton.onClick.AddListener(OnGoOnline);

        SetPhoneButtonsInteractable(false);

    }

    private void Update()
    {
        if (!dialogueActive)
            return;

        if (Input.GetMouseButtonDown(0))
{
    if (isTyping)
    {
        SkipTyping();
    }
    else
    {
        Next();
    }
}

    }



    void SetPhoneButtonsInteractable(bool value)
    {
        doomScrollButton.interactable = value;
        dozeOffButton.interactable = value;
        goOnlineButton.interactable = value;

        phoneChoicesActive = value;
    }


    private void SetGameplayUIInteractable(bool value)
    {
        gameplayUICanvas.interactable = value;
        gameplayUICanvas.blocksRaycasts = value;
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        if (musicSource != null)
    originalMusicVolume = musicSource.volume;

        currentDialogue = dialogue;
        segments = dialogue.dialogueSegments;
        index = 0;

        dialogueActive = true;
        dialoguePanel.SetActive(true);
        StartEventStarter();

        if (dialogue.startBackground != null)
        {
            backgroundController.SetOverrideBackground(dialogue.startBackground.sprite);

        }


        SetGameplayUIInteractable(false);
        SetPhoneButtonsInteractable(false);


        ShowLine();
        RoomManager.Instance.SetNavigationEnabled(false);

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

        bool hasName =
            line.character != null &&
            !string.IsNullOrEmpty(line.character.CharacterName);

        if (hasName)
        {
            nameplateRoot.SetActive(true);
            charName.text = line.character.CharacterName;
        }
        else
        {
            nameplateRoot.SetActive(false);
            charName.text = "";
        }

        if (line.character != null &&
            line.character.CharacterSprite != null)
        {
            charSprite.gameObject.SetActive(true);
            charSprite.sprite = line.character.CharacterSprite;
        }
        else
        {
            charSprite.gameObject.SetActive(false);
        }

        StartTyping(line.ActorDialogue);

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
    // EndEventStarter();
    HandleTaskHooks(currentDialogue);

    StartDialogue(currentDialogue.nextDialogue);
    return;
}


        EndDialogue();
    }

    private void ShowDecisionButtons()
    {
        lineFinishedImage?.SetActive(false);

        ApplyPhoneChoicesFromDialogue(currentDialogue);

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
        if (typingRoutine != null)
    StopCoroutine(typingRoutine);
    lineFinishedImage?.SetActive(false);


isTyping = false;

        dialogueActive = false;
        dialoguePanel.SetActive(false);
        decisionPanel.SetActive(false);

        EndEventStarter();

        backgroundController.ClearOverride();


        ApplyPhoneChoicesFromDialogue(currentDialogue);
        HandleTaskHooks(currentDialogue);
        charSprite.gameObject.SetActive(false);
charSprite.sprite = null;

nameplateRoot.SetActive(false);
charName.text = "";
charDialogue.text = "";


        SetGameplayUIInteractable(true);
        if (isNavigationEnabled)
        {
            RoomManager.Instance.SetNavigationEnabled(true);
        }
    }

    void ApplyPhoneChoicesFromDialogue(DialogueSO dialogue)
    {
        bool enable = dialogue.phoneDialogue != null;
        SetPhoneButtonsInteractable(enable);
    }




    void StartEventStarter()
    {
        dialogueEventHandler.TriggerEvent(currentDialogue.startEvent);
    }

    void EndEventStarter()
    {
        dialogueEventHandler.TriggerEvent(currentDialogue.endEvent);
    }

    void HandleTaskHooks(DialogueSO dialogue)
    {
        if (dialogue.activateTask != null)
            TaskManager.Instance.ActivateTask(dialogue.activateTask);

        if (dialogue.completeTask != null)
            TaskManager.Instance.CompleteTask(dialogue.completeTask);

        if (dialogue.failTask != null)
            TaskManager.Instance.FailTask(dialogue.failTask);
    }


    public void OnDoomScroll()
    {
        if (!phoneChoicesActive) return;

        ResolvePhoneChoice(GlobalActionType.DoomScroll);
    }

    public void OnDozeOff()
    {
        if (!phoneChoicesActive) return;

        ResolvePhoneChoice(GlobalActionType.DozeOff);
    }

    public void OnGoOnline()
    {
        if (!phoneChoicesActive) return;

        ResolvePhoneChoice(GlobalActionType.GoOnline);
    }

    void ResolvePhoneChoice(GlobalActionType action)
    {
        // Hide interact/reject if it is open
        if (decisionPanel.activeSelf)
            decisionPanel.SetActive(false);

        // Disable phone buttons immediately
        SetPhoneButtonsInteractable(false);

        // Fire global event
        if (action != GlobalActionType.None)
            dialogueEventHandler.TriggerGlobalAction(action);


        // Continue into phone dialogue
        if (currentDialogue.phoneDialogue != null)
            StartDialogue(currentDialogue.phoneDialogue);
        else
            EndDialogue();
    }





    public void ForceInteract()
    {
        if (!decisionPanel.activeSelf)
            return;

        decisionPanel.SetActive(false);

        // Apply stats FIRST
        StatManager.Instance.ApplyDelta(currentDialogue.interactStats);

        // Fire narrative event
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

    public void ForceReject()
    {
        if (!decisionPanel.activeSelf)
            return;

        decisionPanel.SetActive(false);

        // Apply stats FIRST
        StatManager.Instance.ApplyDelta(currentDialogue.rejectStats);

        // Fire narrative event
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

    void StartTyping(string line)
{
    if (typingRoutine != null)
        StopCoroutine(typingRoutine);

    fullLine = line;

    lineFinishedImage?.SetActive(false);

    typingRoutine = StartCoroutine(TypeRoutine());
}

IEnumerator TypeRoutine()
{
    DuckMusic(true);

    isTyping = true;
    charDialogue.text = "";

    lastSoundTime = -999f;

    foreach (char c in fullLine)
    {
        charDialogue.text += c;

        PlayTypingSound();

        float delay = typeSpeed;

        if (c == ',' || c == ';')
            delay += commaPause;
        else if (c == '.' || c == '!' || c == '?')
            delay += sentencePause;

        yield return new WaitForSeconds(delay);
    }

    isTyping = false;
typingRoutine = null;

lineFinishedImage?.SetActive(true);

    DuckMusic(false);

}


void PlayTypingSound()
{
    if (typingClips.Length == 0 || typeAudioSource == null)
        return;

    if (Time.time - lastSoundTime < soundCooldown)
        return;

    lastSoundTime = Time.time;

    float t = Random.value;
t = t * t; // bias toward upper end

typeAudioSource.pitch = Mathf.Lerp(pitchRange.x, pitchRange.y, t);


    typeAudioSource.PlayOneShot(
    typingClips[Random.Range(0, typingClips.Length)],
    typingVolume
);

}



void SkipTyping()
{
    if (!isTyping)
        return;

    if (typingRoutine != null)
        StopCoroutine(typingRoutine);

    charDialogue.text = fullLine;

    isTyping = false;
    typingRoutine = null;

    lineFinishedImage?.SetActive(true);
}


void DuckMusic(bool duck)
{
    if (musicSource == null)
        return;

    float target = duck ? duckVolume : originalMusicVolume;

    StopCoroutine(nameof(DuckRoutine));
    StartCoroutine(DuckRoutine(target));
}

IEnumerator DuckRoutine(float target)
{
    while (!Mathf.Approximately(musicSource.volume, target))
    {
        musicSource.volume = Mathf.MoveTowards(
            musicSource.volume,
            target,
            duckSpeed * Time.deltaTime);

        yield return null;
    }
}








}
