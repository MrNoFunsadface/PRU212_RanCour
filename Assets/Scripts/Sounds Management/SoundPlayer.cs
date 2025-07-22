using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void playFootStep()
    {
        SoundManager.PlaySound(SoundEffectType.PLAYERMOVEMENT, 1);
    }

    public void playDoorInteraction()
    {
        SoundManager.PlaySound(SoundEffectType.DOORINTERACTION, 1);
    }
}
