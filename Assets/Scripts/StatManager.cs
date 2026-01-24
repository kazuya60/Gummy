using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;

    [Header("Bars")]
    public StatBar socialStanding;
    public StatBar socialAnxiety;
    public StatBar gummyMeter;

    [Header("Game Over")]
    public DialogueSO gummyGameOverDialogue;

    private void Awake()
    {
        Instance = this;
    }

    public void ModifySocialStanding(float delta)
    {
        socialStanding.SetValue(socialStanding.Value + delta);
    }

    public void ModifySocialAnxiety(float delta)
    {
        socialAnxiety.SetValue(socialAnxiety.Value + delta);
    }

    public void ModifyGummy(float delta)
    {
        gummyMeter.SetValue(gummyMeter.Value + delta);

        if (gummyMeter.Value >= 1f)
        {
            TriggerGummyGameOver();
        }
    }

    void TriggerGummyGameOver()
    {
        DialogueManager.Instance.StartDialogue(gummyGameOverDialogue);
    }
}
