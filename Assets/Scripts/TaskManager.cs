using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<TaskSO> allTasks;

    private void Awake()
    {
        Instance = this;

        foreach (var t in allTasks)
            t.state = TaskState.Inactive;
    }

    public void ActivateTask(TaskSO task)
    {
        if (task == null) return;

        task.state = TaskState.Active;
        TaskUI.Instance.Refresh();
    }

    public void CompleteTask(TaskSO task)
    {
        if (task == null) return;

        task.state = TaskState.Completed;
        TaskUI.Instance.Refresh();
    }

    public void FailTask(TaskSO task)
    {
        if (task == null) return;

        task.state = TaskState.Failed;
        TaskUI.Instance.Refresh();
    }
}
