using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public DialogueSO dialogueSO;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI charDialogue;
    public Image charSprite;
    private int dialogueLineIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DialogueStarter()
    {

        if (dialogueLineIndex >= dialogueSO.DialogueScriptableObject.DialogueLines.Length)
        {
           return; 
        }
        DialogueInit(dialogueLineIndex);
        dialogueLineIndex++;
        Debug.Log("Available Dialogues " + dialogueSO.DialogueScriptableObject.DialogueLines.Length + " and current index is" + dialogueLineIndex);

    }

    private void DialogueInit(int index)
    {
        charName.text = dialogueSO.DialogueScriptableObject.DialogueLines[index].character.CharacterName;
        charSprite.sprite = dialogueSO.DialogueScriptableObject.DialogueLines[index].character.CharacterSprite;
        charDialogue.text = dialogueSO.DialogueScriptableObject.DialogueLines[index].ActorDialogue;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
