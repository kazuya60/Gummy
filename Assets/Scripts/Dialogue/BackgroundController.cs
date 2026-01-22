using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;

    public void SetBackground(Sprite sprite)
    {
        if (sprite == null) return;
        backgroundImage.sprite = sprite;
        backgroundImage.color = Color.white;
        backgroundImage.transform.localScale = Vector3.one;
    }

    
}
