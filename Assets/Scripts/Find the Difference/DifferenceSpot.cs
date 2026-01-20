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
    if (found) return;

    found = true;

    RectTransform rect = transform as RectTransform;

    Vector2 localPos;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        rect,
        eventData.position,
        eventData.pressEventCamera,
        out localPos
    );

    manager.SpawnIcon(
    manager.correctIconPrefab,
    rect
);



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

}
