using UnityEngine;

public class EButton : MonoBehaviour
{
    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
