using UnityEngine;

public class AudioPanel : MonoBehaviour
{
    void Start()
    {
        Hide();
    }

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);

    public void OnAudioButtonClick()
    {
        if (gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
        SoundManager.PlaySound(SoundEffectType.BUTTONCLICK);
    }
}
