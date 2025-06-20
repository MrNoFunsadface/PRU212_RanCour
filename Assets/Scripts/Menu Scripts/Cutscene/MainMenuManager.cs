using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private string cutsceneSceneName = "IntroCutscene";

    [Header("Transition")]
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private float fadeTime = 1f;

    private void Start()
    {
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(StartGame);
        }

        // Ensure fade panel starts invisible
        if (fadePanel != null)
        {
            fadePanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        StartCoroutine(LoadCutsceneWithFade());
    }

    private System.Collections.IEnumerator LoadCutsceneWithFade()
    {
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
            yield return StartCoroutine(FadeIn());
        }

        // Set flag for cutscene manager
        PlayerPrefs.SetInt("PlayIntroCutscene", 1);
        SceneManager.LoadScene(cutsceneSceneName);
    }

    private System.Collections.IEnumerator FadeIn()
    {
        CanvasGroup canvasGroup = fadePanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = fadePanel.AddComponent<CanvasGroup>();
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}