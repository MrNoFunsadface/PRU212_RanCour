using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables; // Add for Timeline support
using UnityEngine.Timeline;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Transition Settings")]
    [SerializeField] private float fadeSpeed = 1.5f;
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private bool useTimeline = true; // Toggle between Timeline and fade

    [Header("Timeline Settings")]
    [SerializeField] private PlayableDirector transitionDirector; // Reference to PlayableDirector
    [SerializeField] private PlayableAsset enterTransitionTimeline; // Timeline for entering scenes
    [SerializeField] private PlayableAsset exitTransitionTimeline; // Timeline for exiting scenes
    [SerializeField] private float timelineExitPoint = 0.9f; // When to load the new scene (0-1)

    [Header("UI References")]
    [SerializeField] private Canvas transitionCanvas;
    [SerializeField] private Image fadeImage;

    private bool isTransitioning = false;
    private string targetSceneName;
    private Action onBeforeSceneLoad;
    private Action onAfterSceneLoad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Setup fade transition components
            SetupFadeComponents();

            // Setup timeline components
            SetupTimelineComponents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupFadeComponents()
    {
        if (transitionCanvas == null)
        {
            Debug.LogError("Transition Canvas is missing!");
        }
        else
        {
            transitionCanvas.sortingOrder = 999;
            transitionCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        if (fadeImage == null && transitionCanvas != null)
        {
            Debug.LogError("Fade Image is missing!");
        }
        else if (fadeImage != null)
        {
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
        }
    }

    private void SetupTimelineComponents()
    {
        if (useTimeline)
        {
            if (transitionDirector == null)
            {
                Debug.LogError("PlayableDirector is missing! Timeline transitions won't work.");
            }

            if (enterTransitionTimeline == null || exitTransitionTimeline == null)
            {
                Debug.LogError("Timeline assets are missing! Assign enter/exit timeline assets.");
            }

            // Add listener for timeline completion
            if (transitionDirector != null)
            {
                transitionDirector.stopped += OnTimelineFinished;
            }
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        if (director == transitionDirector)
        {
            // If this was the enter transition (after loading new scene)
            if (targetSceneName == SceneManager.GetActiveScene().name)
            {
                isTransitioning = false;
            }
        }
    }

    // Method to transition to a new scene with Timeline or fade effect
    public void TransitionToScene(string sceneName, Action beforeSceneLoad = null, Action afterSceneLoad = null)
    {
        if (!isTransitioning)
        {
            targetSceneName = sceneName;
            onBeforeSceneLoad = beforeSceneLoad;
            onAfterSceneLoad = afterSceneLoad;

            if (useTimeline && transitionDirector != null && exitTransitionTimeline != null)
            {
                StartCoroutine(TimelineTransitionRoutine());
            }
            else
            {
                StartCoroutine(FadeTransitionRoutine());
            }
        }
    }

    private IEnumerator TimelineTransitionRoutine()
    {
        isTransitioning = true;

        // Play exit timeline
        transitionDirector.playableAsset = exitTransitionTimeline;
        transitionDirector.Play();

        // Wait until we reach the exit point in the timeline
        float timeToWait = (float)(timelineExitPoint * exitTransitionTimeline.duration);
        yield return new WaitForSeconds(timeToWait);

        // Execute pre-load actions
        onBeforeSceneLoad?.Invoke();

        // Load the new scene
        SceneManager.LoadScene(targetSceneName);

        // Wait a frame for scene to initialize
        yield return null;

        // Execute post-load actions
        onAfterSceneLoad?.Invoke();

        // Play enter timeline
        transitionDirector.Stop();
        transitionDirector.playableAsset = enterTransitionTimeline;
        transitionDirector.Play();

        // Timeline completion will set isTransitioning = false via OnTimelineFinished callback
    }

    private IEnumerator FadeTransitionRoutine()
    {
        isTransitioning = true;

        // Fade out
        yield return StartCoroutine(FadeOut());

        // Execute any actions before scene load
        onBeforeSceneLoad?.Invoke();

        // Load the new scene
        SceneManager.LoadScene(targetSceneName);

        // Wait a frame to make sure the scene is loaded
        yield return null;

        // Execute any actions after scene load
        onAfterSceneLoad?.Invoke();

        // Fade in
        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }

    // Original fade methods (kept for backward compatibility)
    private IEnumerator FadeOut()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            float alpha = fadeCurve.Evaluate(t);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1);
    }

    private IEnumerator FadeIn()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime * fadeSpeed;
            float alpha = fadeCurve.Evaluate(t);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
    }
}