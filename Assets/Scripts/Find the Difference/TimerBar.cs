using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public float totalTime = 60f; // seconds

    private float timeRemaining;
    private Image fillImage;
    private bool isRunning;

    void Awake()
    {
        fillImage = GetComponent<Image>();
    }

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (!isRunning || FindObjectOfType<FindDifference>().IsGameEnded)
    return;


        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0f);

        fillImage.fillAmount = timeRemaining / totalTime;

        if (timeRemaining <= 0f)
        {
            isRunning = false;
            OnTimerEnded();
        }
    }

    public void StartTimer()
    {
        timeRemaining = totalTime;
        fillImage.fillAmount = 1f;
        isRunning = true;
    }

    private void OnTimerEnded()
    {
        Debug.Log("Time's up!");
        FindObjectOfType<FindDifference>()?.OnTimeUp();
    }
}
