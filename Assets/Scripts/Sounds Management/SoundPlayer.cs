using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void playFootStep()
    {
        SoundManager.PlaySound(SoundEffectType.PLAYERMOVEMENT);
    }

    public void playDoorInteraction()
    {
        SoundManager.PlaySound(SoundEffectType.DOORINTERACTION);
    }
}
