using Scripts.Models;
using System;
using UnityEngine;

public class ViewDeckButton : MonoBehaviour
{
    [SerializeField]
    private CardView cardView;

    void Start()
    {
        Hide();
    }

    public void OnViewDeckButtonClicked()
    {
        cardView.Show();
    }

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
