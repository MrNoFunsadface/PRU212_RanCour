using System;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    private void Awake()
    {
        Hide();
    }

    public Boolean getState() => gameObject.activeSelf;

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}
