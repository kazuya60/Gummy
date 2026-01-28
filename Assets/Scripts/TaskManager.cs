using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static System.Action OnTasksChanged;

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

    OnTasksChanged?.Invoke();

    if (TaskUI.Instance != null)
        TaskUI.Instance.Refresh();
}


    public void CompleteTask(TaskSO task)
{
    if (task == null) return;

    task.state = TaskState.Completed;

    OnTasksChanged?.Invoke();

    if (TaskUI.Instance != null)
        TaskUI.Instance.Refresh();
}


   public void FailTask(TaskSO task)
{
    if (task == null) return;

    task.state = TaskState.Failed;

    OnTasksChanged?.Invoke();

    if (TaskUI.Instance != null)
        TaskUI.Instance.Refresh();
}

}
