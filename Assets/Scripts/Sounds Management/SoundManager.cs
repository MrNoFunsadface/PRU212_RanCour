using System;
using Unity.VisualScripting;
using UnityEngine;

public enum SoundEffectType
{
    PLAYERMOVEMENT,
    BATATTACK,
    SLIMEATTACK,
    DOORINTERACTION,
    ITEMPICKUP,
    CARDSELECTION,
    CARDAPPLICATION,
    DAMAGETAKING,
    BUTTONCLICK,
    MENUOPEN,
    ENDTURN,
    KNIGHTATTACK,
}

public enum SoundTrackList
{
    SCENE0,
    SCENE1,
    SCENE2,
    SCENE3,
    BATTLENORMAL,
    KNIGHTBOSSBATTLE,
    MAINMENU,
    SILENCE,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private SoundList[] soundList;
    [SerializeField] private SoundTrack[] soundTracks;
    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private float musicVolume = 1f;
    private AudioSource musicSource;  // Renamed to clarify purpose
    [SerializeField] private AudioSource sfxSource;  // New dedicated source for sound effects

    // Add this static property to check if the instance is ready
    public static bool IsInitialized => Instance != null && Instance.musicSource != null;

    private void Awake()
    {
        // Only execute singleton pattern in play mode, not in edit mode
        if (!Application.isPlaying)
        {
            // Skip singleton setup in edit mode
            musicSource = GetComponent<AudioSource>();
            return;
        }

        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep the SoundManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }


        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
        {
            Debug.LogError("AudioSource component is missing on SoundManager GameObject.");
        }

        // Create a separate AudioSource for sound effects if it doesn't exist
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(SoundEffectType sound)
    {
        float volume = Instance.masterVolume * Instance.sfxVolume;
        AudioClip[] clips = Instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance.sfxSource.PlayOneShot(randomClip, volume);  // Use sfxSource instead
    }

    public void PlaySoundTrack(SoundTrackList soundTrack)
    {
        float volume = Instance.masterVolume * Instance.musicVolume;
        AudioClip track = Instance.soundTracks[(int)soundTrack].Sounds;
        if (track != null)
        {
            Instance.musicSource.clip = track;
            Instance.musicSource.loop = true;
            Instance.musicSource.volume = volume;
            Instance.musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Sound track {soundTrack} is not assigned or missing.");
        }
    }

    public void StopSoundTrack()
    {
        if (Instance.musicSource.isPlaying)
        {
            Instance.musicSource.Stop();
        }
    }

    public float MasterVolume
    {
        get => Instance != null ? Instance.masterVolume : 1f;
        set
        {
            if (Instance == null)
            {
                return;
            }

            Instance.masterVolume = Mathf.Clamp01(value);
            UpdateVolumeSettings();
        }
    }

    public float SFXVolume
    {
        get => Instance.sfxVolume;
        set
        {
            Instance.sfxVolume = Mathf.Clamp01(value);
            UpdateVolumeSettings();
        }
    }

    public float MusicVolume
    {
        get => Instance.musicVolume;
        set
        {
            Instance.musicVolume = Mathf.Clamp01(value);
            UpdateVolumeSettings();
        }
    }

    private void UpdateVolumeSettings()
    {
        if (Instance == null)
        {
            Debug.LogWarning("SoundManager instance is not initialized. Cannot update volume settings.");
            return;
        }
        // Update the AudioSource volumes based on the current settings
        Instance.musicSource.volume = Instance.masterVolume * Instance.musicVolume;
        Instance.sfxSource.volume = Instance.masterVolume * Instance.sfxVolume;
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundEffectType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < names.Length; i++)
        {
            soundList[i].SoundName = names[i];
        }

        string[] tracknames = Enum.GetNames(typeof(SoundTrackList));
        Array.Resize(ref soundTracks, tracknames.Length);
        for (int i = 0; i < tracknames.Length; i++)
        {
            soundTracks[i].SoundTrackName = tracknames[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    [HideInInspector] public string SoundName;
    [SerializeField] private AudioClip[] sounds;
    public AudioClip[] Sounds
    {
        get => sounds;
    }
}

[Serializable]
public struct SoundTrack
{
    [HideInInspector] public string SoundTrackName;
    [SerializeField] private AudioClip soundTracks;

    public AudioClip Sounds
    {
        get => soundTracks;
    }
}