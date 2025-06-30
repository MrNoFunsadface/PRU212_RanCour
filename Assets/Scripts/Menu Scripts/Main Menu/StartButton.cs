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
        PlayerPrefs.DeleteAll();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Cutscene0");
    }
}