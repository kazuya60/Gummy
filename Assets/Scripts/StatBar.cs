using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    [Range(0f, 1f)]
    [SerializeField] private float value;

    public float Value => value;

    private void Awake()
    {
        if (fillImage == null)
            fillImage = GetComponent<Image>();

        SetValue(value);
    }

    public void SetValue(float newValue)
    {
        value = Mathf.Clamp01(newValue);
        fillImage.fillAmount = value;
    }
}
