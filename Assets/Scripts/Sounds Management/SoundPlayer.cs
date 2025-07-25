using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void playFootStep()
    {
        SoundManager.Instance.PlaySound(SoundEffectType.PLAYERMOVEMENT);
    }

    public void playDoorInteraction()
    {
        SoundManager.Instance.PlaySound(SoundEffectType.DOORINTERACTION);
    }
}
