using UnityEngine;
using UnityEngine.EventSystems;

public class WrongClickCatcher : MonoBehaviour, IPointerClickHandler
{
    private FindDifference manager;

    void Awake()
    {
        manager = FindObjectOfType<FindDifference>();
    }

    public void OnPointerClick(PointerEventData eventData)
{
    if (manager.IsGameEnded)
        return;

    RectTransform rect = transform as RectTransform;

    Vector2 localPos;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        rect,
        eventData.position,
        eventData.pressEventCamera,
        out localPos
    );

    manager.SpawnIcon(manager.wrongIconPrefab, rect, localPos);
    manager.OnWrongClick();
}

}
