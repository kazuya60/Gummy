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
        manager.OnDifferenceFound(diffRoot);
    }

    public void DisableAll()
    {
        // Disable ALL hit areas under this difference
        foreach (var img in diffRoot.GetComponentsInChildren<Image>())
        {
            img.raycastTarget = false;
            img.enabled = false; // optional visual feedback
        }
    }
}
