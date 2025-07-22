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
    ITEMPICKUP
}

public enum SoundTrackList
{
    SCENE0,
    SCENE1,
    BATTLENORMAL
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private SoundTrack[] soundTracks;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on SoundManager GameObject.");
        }
    }

    public static void PlaySound(SoundEffectType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

    public static void PlaySoundTrack(SoundTrackList soundTrack)
    {
        AudioClip track = instance.soundTracks[(int)soundTrack].Sounds;
        if (track != null)
        {
            instance.audioSource.clip = track;
            instance.audioSource.loop = true;
            instance.audioSource.Play();
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