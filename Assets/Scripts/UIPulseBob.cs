using UnityEngine;

public class UIPulseBob : MonoBehaviour
{
    [Header("Bob")]
    public float bobAmount = 8f;
    public float bobSpeed = 2f;

    [Header("Pulse")]
    public float pulseAmount = 0.08f;
    public float pulseSpeed = 2.5f;

    Vector3 startPos;
    Vector3 startScale;

    void Awake()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localPosition = startPos;
        transform.localScale = startScale;
    }

    void Update()
    {
        float t = Time.time;

        float y = Mathf.Sin(t * bobSpeed) * bobAmount;
        transform.localPosition = startPos + Vector3.up * y;

        float s = 1f + Mathf.Sin(t * pulseSpeed) * pulseAmount;
        transform.localScale = startScale * s;
    }
}
