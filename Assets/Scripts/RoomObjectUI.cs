using UnityEngine;

public class RoomObjectUI : MonoBehaviour
{
    public RoomSO visibleInRoom;
    public TaskSO requiredTask;

    [Header("Visual To Toggle")]
    public GameObject visualRoot;

    private void OnEnable()
    {
        RoomManager.OnRoomChanged += HandleRoomChanged;
        TaskManager.OnTasksChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        RoomManager.OnRoomChanged -= HandleRoomChanged;
        TaskManager.OnTasksChanged -= Refresh;
    }

    void HandleRoomChanged(RoomSO room)
    {
        Refresh();
    }

    void Refresh()
    {
        if (visualRoot == null ||
            RoomManager.Instance == null ||
            TaskManager.Instance == null)
            return;

        bool correctRoom =
            RoomManager.Instance.CurrentRoom == visibleInRoom;

        bool taskActive =
            requiredTask != null &&
            requiredTask.state == TaskState.Active;

        visualRoot.SetActive(correctRoom && taskActive);
    }
}
