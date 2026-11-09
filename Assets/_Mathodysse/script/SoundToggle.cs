using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public Image icon;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    void Start() => Refresh();

    public void Toggle()
    {
        MusicManager.Instance.SetMusic(!MusicManager.Instance.IsOn());
        Refresh();
    }

    void Refresh()
    {
        icon.sprite = MusicManager.Instance.IsOn() ? soundOnSprite : soundOffSprite;
    }
}