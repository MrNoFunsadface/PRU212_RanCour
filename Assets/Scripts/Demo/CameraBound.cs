using UnityEngine;

public class CameraBound : MonoBehaviour
{
    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
}
