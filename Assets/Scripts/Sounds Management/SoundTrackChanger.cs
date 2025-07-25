using UnityEngine;

public class SoundTrackChanger : MonoBehaviour
{
    [SerializeField] private bool debugMode = false; // Enable debug mode for logging

    public void Start()
    {
        // Check if the GameManager instance is available
        if (GameManager.Instance != null)
        {
            // Set the sound track for the GameManager
            SoundTrackPlayer.Instance.SetSoundTrack(GameManager.Instance.soundTrack);
            if (debugMode) Debug.Log($"[SoundTrackChanger] Sound track set to: {GameManager.Instance.soundTrack}");
        }
        else
        {
            if (debugMode) Debug.LogWarning("[SoundTrackChanger] GameManager instance is not available. Cannot set sound track.");
        }
    }
}
