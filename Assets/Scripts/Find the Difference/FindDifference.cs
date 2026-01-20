using UnityEngine;
using System.Collections.Generic;

public class FindDifference : MonoBehaviour
{
    public Transform differencesParent;
    public int chances = 3;

    private int totalDifferences;
    private int foundDifferences;
    private bool gameEnded;
    public bool IsGameEnded => gameEnded;


    public GameObject correctIconPrefab;
    public GameObject wrongIconPrefab;


    private HashSet<Transform> foundRoots = new HashSet<Transform>();

    void Start()
    {
        totalDifferences = differencesParent.childCount;
    }

    public void OnDifferenceFound(Transform diffRoot)
    {
        if (gameEnded) return;

        if (foundRoots.Contains(diffRoot))
            return;

        foundRoots.Add(diffRoot);
        foundDifferences++;

        foreach (DifferenceSpot spot in diffRoot.GetComponentsInChildren<DifferenceSpot>())
        {
            spot.DisableAll();
        }

        Debug.Log("Difference found!");

        if (foundDifferences >= totalDifferences)
        {
            gameEnded = true;
            Debug.Log("ALL DIFFERENCES FOUND!");
        }
    }

    public void OnWrongClick()
    {
        if (gameEnded) return;

        chances--;
        Debug.Log("Wrong click. Chances left: " + chances);

        if (chances <= 0)
        {
            gameEnded = true;
            Debug.Log("GAME OVER");
        }
    }

    public void SpawnIcon(GameObject prefab, Transform parent, Vector2 localPosition)
{
    GameObject icon = Instantiate(prefab, parent);
    RectTransform rect = icon.GetComponent<RectTransform>();

    rect.anchoredPosition = localPosition;

    // Auto destroy (for X mark)
    Destroy(icon, 0.6f);
}



}
