using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeSceneChanger : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public string sceneToLoad;

    private bool isFading = false;

    public void StartGame()
    {
        if (!isFading)
            StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        isFading = true;

        // Fade to black
        float t = 0f;
        Color color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Load the scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
