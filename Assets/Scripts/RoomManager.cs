using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public RoomSO startingRoom;
    private RoomSO currentRoom;

    public BackgroundController backgroundController;

    public ArrowUI arrows;

    private void Awake()
    {
        Instance = this;
        EnterRoom(startingRoom);
        SetNavigationEnabled(false);
    }

    public void EnterRoom(RoomSO room)
{
    if (currentRoom != null &&
        currentRoom.onExitEvent != DialogueEventType.None)
    {
        DialogueManager.Instance.dialogueEventHandler
            .TriggerEvent(currentRoom.onExitEvent);
    }

    currentRoom = room;

    backgroundController.SetRoomBackground(room.roomBackground);

    arrows.Refresh(room);

    if (room.onEnterEvent != DialogueEventType.None)
    {
        DialogueManager.Instance.dialogueEventHandler
            .TriggerEvent(room.onEnterEvent);
    }
}


    public RoomSO CurrentRoom => currentRoom;
    public void SetNavigationEnabled(bool value)
{
    arrows.gameObject.SetActive(value);
}

}
