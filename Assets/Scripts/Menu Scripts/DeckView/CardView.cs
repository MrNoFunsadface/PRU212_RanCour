using UnityEngine;

public class CardView : MonoBehaviour
{
    void Start()
    {
        Hide();
    }

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
