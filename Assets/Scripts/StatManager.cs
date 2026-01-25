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

    void Start()
    {
        // socialStanding.SetValue(12);
        // socialAnxiety.SetValue(12);
        // gummyMeter.SetValue(20);
        
    }

    public void AddSocialStanding(int amount)
    {
        socialStanding.Modify(amount);
    }

    public void AddSocialAnxiety(int amount)
    {
        socialAnxiety.Modify(amount);
    }

    public void AddGummy(int amount)
    {
        gummyMeter.Modify(amount);

        if (gummyMeter.Current >= gummyMeter.Max)
        {
            TriggerGummyGameOver();
        }
    }

    void TriggerGummyGameOver()
    {
        DialogueManager.Instance.StartDialogue(gummyGameOverDialogue);
    }
}
