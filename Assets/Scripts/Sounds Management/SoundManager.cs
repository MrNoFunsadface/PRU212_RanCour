using Ink;
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
}

public enum SoundTrackList
{
    SCENE0,
    SCENE1,
    BATTLENORMAL,
    KNIGHTBOSSBATTLE
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance;

    [SerializeField] private SoundList[] soundList;
    [SerializeField] private SoundTrack[] soundTracks;
    
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on SoundManager GameObject.");
        }
    }

    public static void PlaySound(SoundEffectType sound, float volume = 1)
    {
        AudioClip[] clips = Instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance.audioSource.PlayOneShot(randomClip, volume);
    }

    public static void PlaySoundTrack(SoundTrackList soundTrack, float volume = 1)
    {
        AudioClip track = Instance.soundTracks[(int)soundTrack].Sounds;
        if (track != null)
        {
            Instance.audioSource.clip = track;
            Instance.audioSource.loop = true;
            Instance.audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Sound track {soundTrack} is not assigned or missing.");
        }
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