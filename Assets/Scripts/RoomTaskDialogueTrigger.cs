using UnityEngine;

public class RoomTaskDialogueTrigger : MonoBehaviour
{
    public RoomSO room;
    public TaskSO requiredTask;
    public DialogueSO dialogueToStart;

    private bool fired;

    private void OnEnable()
    {
        RoomManager.OnRoomChanged += HandleRoomChanged;
        TaskManager.OnTasksChanged += HandleTasksChanged;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomChanged -= HandleRoomChanged;
        TaskManager.OnTasksChanged -= HandleTasksChanged;
    }

    void HandleRoomChanged(RoomSO newRoom)
    {
        TryTrigger(newRoom);
    }

    void HandleTasksChanged()
    {
        TryTrigger(RoomManager.Instance.CurrentRoom);
    }

    void TryTrigger(RoomSO currentRoom)
    {
        if (fired) return;

        if (currentRoom != room)
            return;

        if (requiredTask == null ||
            requiredTask.state != TaskState.Active)
            return;

        fired = true;

        DialogueManager.Instance.StartDialogue(dialogueToStart);
    }
}
