using UnityEngine;
using UnityEngine.UI;

public class ArrowUI : MonoBehaviour
{
    public Button up;
    public Button down;
    public Button right;
    public Button left;

    public void Refresh(RoomSO room)
    {
        up.gameObject.SetActive(room.up != null);
        down.gameObject.SetActive(room.down != null);
        right.gameObject.SetActive(room.right != null);
        left.gameObject.SetActive(room.left != null);

        up.onClick.RemoveAllListeners();
        down.onClick.RemoveAllListeners();
        right.onClick.RemoveAllListeners();
        left.onClick.RemoveAllListeners();

        if (room.up != null)
            up.onClick.AddListener(() => RoomManager.Instance.EnterRoom(room.up));

        if (room.down != null)
            down.onClick.AddListener(() => RoomManager.Instance.EnterRoom(room.down));

        if (room.right != null)
            right.onClick.AddListener(() => RoomManager.Instance.EnterRoom(room.right));

        if (room.left != null)
            left.onClick.AddListener(() => RoomManager.Instance.EnterRoom(room.left));
    }
}
