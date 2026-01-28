using UnityEngine;

public enum TaskState
{
    Inactive,
    Active,
    Completed,
    Failed
}

[CreateAssetMenu(menuName = "Game/Task")]
public class TaskSO : ScriptableObject
{
    public string title;

    [HideInInspector]
    public TaskState state = TaskState.Inactive;
}
