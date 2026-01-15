using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{

    private Image fillArea;
    [Range(0f, 1f)]
    public float fillValue;
    // Start is called before the first frame update
    void Start()
    {
        fillArea = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        fillArea.fillAmount = fillValue;
    }
}
