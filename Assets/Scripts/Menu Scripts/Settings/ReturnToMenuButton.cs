using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundEffectType.BUTTONCLICK);
        // Load the main menu scene
        SceneManager.LoadScene("Main Menu");
    }
}
