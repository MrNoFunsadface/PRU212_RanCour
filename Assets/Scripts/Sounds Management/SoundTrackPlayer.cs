using Unity.VisualScripting;
using UnityEngine;

public class SoundTrackPlayer : MonoBehaviour
{
    [SerializeField] SoundTrackList soundTrackList;

    public void Start()
    {
        SoundManager.PlaySoundTrack(soundTrackList);
    }
}
