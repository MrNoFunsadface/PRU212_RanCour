using UnityEngine;

public class FogBarrel : MonoBehaviour
{
    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
