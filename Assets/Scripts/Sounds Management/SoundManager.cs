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
    KNIGHTBOSSBATTLE
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance;

    [SerializeField] private SoundList[] soundList;
    [SerializeField] private SoundTrack[] soundTracks;
    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private float musicVolume = 1f;
    private static SoundManager instance;
    private AudioSource musicSource;  // Renamed to clarify purpose
    [SerializeField] private AudioSource sfxSource;  // New dedicated source for sound effects

    // Add this static property to check if the instance is ready
    public static bool IsInitialized => instance != null && instance.musicSource != null;

    private void Awake()
    {
        // Only execute singleton pattern in play mode, not in edit mode
        if (!Application.isPlaying)
        {
            // Skip singleton setup in edit mode
            musicSource = GetComponent<AudioSource>();
            return;
        }

        // Singleton pattern with DontDestroyOnLoad (only in play mode)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

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

    public static void PlaySound(SoundEffectType sound)
    {
        float volume = instance.masterVolume * instance.sfxVolume;
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.sfxSource.PlayOneShot(randomClip, volume);  // Use sfxSource instead
    }

    public static void PlaySoundTrack(SoundTrackList soundTrack)
    {
        float volume = instance.masterVolume * instance.musicVolume;
        AudioClip track = instance.soundTracks[(int)soundTrack].Sounds;
        if (track != null)
        {
            instance.musicSource.clip = track;
            instance.musicSource.loop = true;
            instance.musicSource.volume = volume;
            instance.musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Sound track {soundTrack} is not assigned or missing.");
        }
    }

    public static float MasterVolume
    {
        get => instance != null ? instance.masterVolume : 1f;
        set
        {
            if (instance == null)
            {
                return;
            }

            instance.masterVolume = Mathf.Clamp01(value);
            UpdateVolumeSettings();
        }
    }

    public static float SFXVolume
    {
        get => instance.sfxVolume;
        set
        {
            instance.sfxVolume = Mathf.Clamp01(value);
            UpdateVolumeSettings();
        }
    }

    public static float MusicVolume
    {
        get => instance.musicVolume;
        set
        {
            instance.musicVolume = Mathf.Clamp01(value);
            UpdateVolumeSettings();
        }
    }

    private static void UpdateVolumeSettings()
    {
        if (instance == null)
        {
            Debug.LogWarning("SoundManager instance is not initialized. Cannot update volume settings.");
            return;
        }
        // Update the AudioSource volumes based on the current settings
        instance.musicSource.volume = instance.masterVolume * instance.musicVolume;
        instance.sfxSource.volume = instance.masterVolume * instance.sfxVolume;
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