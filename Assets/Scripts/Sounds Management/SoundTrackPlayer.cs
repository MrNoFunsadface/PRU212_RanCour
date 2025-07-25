using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundTrackPlayer : MonoBehaviour
{
    public static SoundTrackPlayer Instance { get; private set; }

    [SerializeField] SoundTrackList soundTrack;

    public void Start()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        SoundManager.Instance.PlaySoundTrack(soundTrack);
    }

    public void SetSoundTrack(SoundTrackList newSoundTrack)
    {
        soundTrack = newSoundTrack;
        SoundManager.Instance.StopSoundTrack();
        SoundManager.Instance.PlaySoundTrack(soundTrack);
    }
}