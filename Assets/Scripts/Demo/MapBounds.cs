using UnityEngine;

public class MapBounds : MonoBehaviour
{
    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
}
