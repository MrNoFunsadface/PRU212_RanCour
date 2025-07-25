using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

/// <summary>
/// Listens for a keystroke to skip a timeline-based cutscene with a smooth transition animation.
/// </summary>
public class SkipCutsceneKeystrokeListener : MonoBehaviour
{
    [Header("Timeline Settings")]
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private float skipToTime = -1f; // -1 means skip to end
    
    [Header("Input Settings")]
    [SerializeField] private Key skipKey = Key.Space;
    [SerializeField] private Key alternateSkipKey = Key.Escape;
    
    [Header("Transition Settings")]
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private string transitionTrigger = "FadeOut";
    [SerializeField] private float transitionDelay = 1f;
    [SerializeField] private bool disableListenerAfterSkip = true;
    
    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private bool isSkipping = false;
    private bool hasSkipped = false;

    private void Awake()
    {
        // Validate required components
        if (timeline == null)
        {
            Debug.LogError("Timeline not assigned to SkipCutsceneKeystrokeListener");
            enabled = false;
            return;
        }

        if (transitionAnimator == null)
        {
            Debug.LogWarning("Transition Animator not assigned to SkipCutsceneKeystrokeListener - transitions will be skipped");
        }
    }

    private void Update()
    {
        // Don't process input if already skipping or has skipped
        if (isSkipping || hasSkipped || timeline == null)
            return;

        // Check for skip key press
        if (Keyboard.current[skipKey].wasPressedThisFrame || 
            Keyboard.current[alternateSkipKey].wasPressedThisFrame)
        {
            if (debugMode)
                Debug.Log("Skip key pressed, initiating cutscene skip");
                
            StartCoroutine(SkipCutsceneWithTransition());
        }
    }

    private IEnumerator SkipCutsceneWithTransition()
    {
        isSkipping = true;

        // Play transition animation if available
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger(transitionTrigger);
            
            if (debugMode)
                Debug.Log($"Playing transition animation with trigger: {transitionTrigger}");
                
            // Wait for transition animation to play
            yield return new WaitForSeconds(transitionDelay);
        }

        // Skip to the specified time or end of timeline
        if (skipToTime >= 0 && skipToTime < timeline.duration)
        {
            if (debugMode)
                Debug.Log($"Skipping timeline to time: {skipToTime}");
                
            timeline.time = skipToTime;
            timeline.Play(); // Resume playback from the new time
        }
        else
        {
            if (debugMode)
                Debug.Log("Skipping timeline to end");
                
            // Skip to the end (or very close to it)
            timeline.time = timeline.duration - 0.1f;
            timeline.Play(); // Resume playback from the new time
        }

        hasSkipped = true;
        
        // Optionally disable this component to prevent further skips
        if (disableListenerAfterSkip)
        {
            if (debugMode)
                Debug.Log("Disabling skip listener");
                
            enabled = false;
        }
        else
        {
            isSkipping = false;
        }
    }

    /// <summary>
    /// Reset the skip state, allowing the cutscene to be skipped again
    /// </summary>
    public void ResetSkipState()
    {
        isSkipping = false;
        hasSkipped = false;
    }

    /// <summary>
    /// Set a new timeline to listen for skipping
    /// </summary>
    public void SetTimeline(PlayableDirector newTimeline, float newSkipToTime = -1f)
    {
        timeline = newTimeline;
        skipToTime = newSkipToTime;
        ResetSkipState();
    }
}
