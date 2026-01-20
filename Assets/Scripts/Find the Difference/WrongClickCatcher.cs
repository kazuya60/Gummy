using UnityEngine;
using UnityEngine.EventSystems;

public class WrongClickCatcher : MonoBehaviour, IPointerClickHandler
{
    private FindDifference manager;
    public RectTransform mirroredImage;

    void Awake()
    {
        manager = FindObjectOfType<FindDifference>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (manager.IsGameEnded)
            return;

        RectTransform thisRect = transform as RectTransform;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            thisRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPos
        );

        // X on clicked side
        manager.SpawnIcon(
            manager.wrongIconPrefab,
            thisRect,
            localPos,
            0.6f
        );

        // X on mirrored side
        if (mirroredImage != null)
        {
            manager.SpawnIcon(
                manager.wrongIconPrefab,
                mirroredImage,
                localPos,
                0.6f
            );
        }

        manager.OnWrongClick();
    }
}