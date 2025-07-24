using Unity.VisualScripting;
using UnityEngine;

public class ReturnToMenuButton : MonoBehaviour
{
    public void OnButtonClick() => SoundManager.PlaySound(SoundEffectType.BUTTONCLICK);
}
