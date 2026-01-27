using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    private Sprite roomBackground;
private Sprite overrideBackground;


    public void SetBackground(Sprite sprite)
    {
        if (sprite == null) return;
        backgroundImage.sprite = sprite;
        backgroundImage.color = Color.white;
        backgroundImage.transform.localScale = Vector3.one;
    }

    public void SetRoomBackground(Sprite sprite)
{
    roomBackground = sprite;
    SetBackground(sprite);
}

public void SetOverrideBackground(Sprite sprite)
{
    overrideBackground = sprite;
    SetBackground(sprite);
}

public void ClearOverride()
{
    overrideBackground = null;
    SetBackground(roomBackground);
}


    
}
