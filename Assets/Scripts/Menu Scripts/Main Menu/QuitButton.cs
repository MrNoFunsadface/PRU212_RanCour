using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void OnQuitButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundEffectType.BUTTONCLICK);
        Application.Quit();
    }
}