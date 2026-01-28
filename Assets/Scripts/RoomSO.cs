using UnityEngine;

[CreateAssetMenu(fileName = "Room_", menuName = "Rooms/Room")]
public class RoomSO : ScriptableObject
{
    public string roomName;
    public Sprite roomBackground;

    public RoomSO up;
    public RoomSO down;
    public RoomSO right;
    public RoomSO left;

    [Header("Room Events")]
public DialogueEventType onEnterEvent;
public DialogueEventType onExitEvent;

[Header("Task Hooks")]
public TaskSO completeTaskOnEnter;
public TaskSO failTaskOnEnter;


}
