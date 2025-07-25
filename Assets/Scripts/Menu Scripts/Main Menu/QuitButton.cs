using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void OnQuitButtonClick()
    {
        SoundManager.PlaySound(SoundEffectType.BUTTONCLICK);
        Application.Quit();
    }
}