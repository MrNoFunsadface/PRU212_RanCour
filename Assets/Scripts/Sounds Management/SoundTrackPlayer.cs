using Unity.VisualScripting;
using UnityEngine;

public class SoundTrackPlayer : MonoBehaviour
{
    [SerializeField] SoundTrackList soundTrack;

    public void Start()
    {
        SoundManager.Instance.PlaySoundTrack(soundTrack);
    }

    public void SetSoundTrack(SoundTrackList newSoundTrack)
    {
        soundTrack = newSoundTrack;
        SoundManager.Instance.StopSoundTrack();
        SoundManager.Instance.PlaySoundTrack(soundTrack);
    }
}