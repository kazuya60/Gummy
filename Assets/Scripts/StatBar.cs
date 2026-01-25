using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    [SerializeField] private int maxValue = 25;
    [SerializeField] private int currentValue;

    public int Current => currentValue;
    public int Max => maxValue;

    private void Awake()
    {
        
        if (fillImage == null)
            fillImage = GetComponent<Image>();

        SetValue(currentValue);
    }

    public void SetValue(int value)
    {
        currentValue = Mathf.Clamp(value, 0, maxValue);
        fillImage.fillAmount = (float)currentValue / maxValue;
    }

    public void Modify(int delta)
    {
        SetValue(currentValue + delta);
    }
}
