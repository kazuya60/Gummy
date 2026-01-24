using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class FindDifference : MonoBehaviour
{
    public Transform differencesParent;
    public int chances = 3;
    public TextMeshProUGUI chancesText;

    private int totalDifferences;
    private int foundDifferences;
    private bool gameEnded;
    public bool IsGameEnded => gameEnded;

    [Header("Progress UI")]
    public Transform checkboxesParent;
    public Sprite uncheckedSprite;
    public Sprite checkedSprite;

    private Image[] checkboxes;



    public GameObject correctIconPrefab;
    public GameObject wrongIconPrefab;

    [Header("Dialogue Events")]
private DialogueEventType successEvent;
private DialogueEventType failureEvent;



    private HashSet<Transform> foundRoots = new HashSet<Transform>();

    void Start()
    {
        successEvent = DialogueEventType.DifferenceSuccess;
        failureEvent = DialogueEventType.DifferenceFailure;
        totalDifferences = differencesParent.childCount;
        UpdateChancesUI();

        checkboxes = checkboxesParent.GetComponentsInChildren<Image>();

        // Safety: ensure they all start unchecked
        for (int i = 0; i < checkboxes.Length; i++)
        {
            checkboxes[i].sprite = uncheckedSprite;
        }
    }

    private void UpdateChancesUI()
    {
        if (chancesText != null)
            chancesText.text = $"{chances}";
    }

    public void OnTimeUp()
{
    if (gameEnded)
        return;

    EndGame(false);
}





    public void OnDifferenceFound(Transform diffRoot)
    {
        if (gameEnded) return;

        if (foundRoots.Contains(diffRoot))
            return;

        foundRoots.Add(diffRoot);
        foundDifferences++;
        UpdateCheckboxUI();


        foreach (DifferenceSpot spot in diffRoot.GetComponentsInChildren<DifferenceSpot>())
        {
            spot.DisableAll();
        }

        Debug.Log("Difference found!");

        if (foundDifferences >= totalDifferences)
{
    EndGame(true);
}

    }

    public void OnWrongClick()
    {
        if (gameEnded) return;

        chances--;

        UpdateChancesUI();

        Debug.Log("Wrong click. Chances left: " + chances);

        if (chances <= 0)
{
    EndGame(false);
}

    }


    public void SpawnIcon(
    GameObject prefab,
    RectTransform parent,
    Vector2? localPosition = null,
    float lifetime = -1f
)
    {
        GameObject icon = Instantiate(prefab);
        RectTransform rect = icon.GetComponent<RectTransform>();

        rect.SetParent(parent, false);

        // If no position is provided → snap to pivot
        rect.anchoredPosition = localPosition ?? Vector2.zero;

        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one;

        if (lifetime > 0f)
            Destroy(icon, lifetime);
    }

    private void UpdateCheckboxUI()
    {
        for (int i = 0; i < checkboxes.Length; i++)
        {
            if (i < foundDifferences)
                checkboxes[i].sprite = checkedSprite;
            else
                checkboxes[i].sprite = uncheckedSprite;
        }
    }

    private void EndGame(bool success)
{
    if (gameEnded)
        return;

    gameEnded = true;

    if (success)
    {
        Debug.Log("ALL DIFFERENCES FOUND!");
        DialogueManager.Instance.dialogueEventHandler
            .TriggerEvent(successEvent);
    }
    else
    {
        Debug.Log("GAME OVER");
        DialogueManager.Instance.dialogueEventHandler
            .TriggerEvent(failureEvent);
    }
}









}
