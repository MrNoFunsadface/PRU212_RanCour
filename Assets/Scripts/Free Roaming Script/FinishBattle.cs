using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBattle : MonoBehaviour
{
    public void OnFinishBattleClicked()
    {        
        SceneManager.LoadScene("Scene1"); // Your world scene name
        SoundManager.PlaySound(SoundEffectType.BUTTONCLICK);
    }
}
