using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    public Animator transitionAnimator;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string sceneName, string startTrigger, float startAnimationTime)
    {
        // Start the coroutine to load the level with a transition
        StartCoroutine(LoadLevelCoroutine(sceneName, startTrigger, startAnimationTime));
    }

    IEnumerator LoadLevelCoroutine(string sceneName, string startTrigger, float startAnimationTime)
    {
        // Play the start transition animation
        transitionAnimator.SetTrigger(startTrigger);
        // Wait for the specified transition time
        yield return new WaitForSeconds(startAnimationTime);
        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }
}
