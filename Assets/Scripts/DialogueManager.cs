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

        if (dialogueLineIndex >= dialogueSO.dialogueSegments.Length)
        {
           return; 
        }
        DialogueInit(dialogueLineIndex);
        dialogueLineIndex++;
        Debug.Log("Available Dialogues " + dialogueSO.dialogueSegments.Length + " and current index is" + dialogueLineIndex);

    }

    private void DialogueInit(int index)
    {
        charName.text = dialogueSO.dialogueSegments[index].character.CharacterName;
        charSprite.sprite = dialogueSO.dialogueSegments[index].character.CharacterSprite;
        charDialogue.text = dialogueSO.dialogueSegments[index].ActorDialogue;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
