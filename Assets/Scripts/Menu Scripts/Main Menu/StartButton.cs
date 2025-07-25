using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        SoundManager.Instance.PlaySound(SoundEffectType.BUTTONCLICK);
        SoundManager.Instance.PlaySound(SoundEffectType.ENDTURN);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Cutscene0");
    }
}