using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifferenceSpot : MonoBehaviour, IPointerClickHandler
{
    private FindDifference manager;
    private Transform diffRoot;
    private bool found;

    void Awake()
    {
        // The Diff_01 / Diff_02 parent
        diffRoot = transform.parent;

        // Find the manager once (cheap + safe)
        manager = FindObjectOfType<FindDifference>();

        if (manager == null)
        {
            Debug.LogError("FindDifference manager not found in scene.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
{
    if (manager.IsGameEnded)
        return;

    if (found)
        return;

    found = true;

    RectTransform thisRect = transform as RectTransform;
    RectTransform otherRect = GetSiblingSpot();

    manager.SpawnIcon(manager.correctIconPrefab, thisRect);

    if (otherRect != null)
    {
        manager.SpawnIcon(manager.correctIconPrefab, otherRect);
    }

    manager.OnDifferenceFound(diffRoot);
}






    public void DisableAll()
    {
        foreach (var spot in diffRoot.GetComponentsInChildren<DifferenceSpot>())
        {
            var img = spot.GetComponent<Image>();
            if (img != null)
            {
                img.raycastTarget = false;
                img.enabled = false;
            }
        }
    }

    public RectTransform GetSiblingSpot()
{
    foreach (Transform child in diffRoot)
    {
        if (child != transform)
            return child as RectTransform;
    }

    return null;
}


}
