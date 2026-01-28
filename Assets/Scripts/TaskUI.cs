using TMPro;
using UnityEngine;
using System.Linq;

public class TaskUI : MonoBehaviour
{
    public static TaskUI Instance;

    public Transform listRoot;
    public GameObject entryPrefab;

    private void Awake()
{
    Instance = this;
}


    public void Refresh()
    {
        foreach (Transform c in listRoot)
            Destroy(c.gameObject);

        foreach (var t in TaskManager.Instance.allTasks
             .Where(t => t.state == TaskState.Active))
{
    var go = Instantiate(entryPrefab, listRoot);

    var text = go.GetComponentInChildren<TextMeshProUGUI>();
    if (text != null)
        text.text = t.title;
}

    }
}
