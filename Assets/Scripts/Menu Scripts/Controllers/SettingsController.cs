using Assets.Scripts.Inventory;
using Scripts.Menu;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private SettingsUI settingsUI;

    [SerializeField]
    private OptionBar optionBar;

    [SerializeField]
    private MenuPage inventoryUI;

    [SerializeField]
    private CharacterUI characterUI;

    [SerializeField]
    private ExitButton exitButton;

    [SerializeField]
    private ViewDeckButton viewDeckButton;

    void Awake()
    {
        optionBar.OnSettingsSelected += HandleSettingsSelected;
    }

    private void HandleSettingsSelected(int obj)
    {
        ShowSettingsUI();
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log("P key pressed, toggling settings UI.");
            if (settingsUI.gameObject.activeSelf)
            {
                HideSettingsUI();
            }
            else
            {
                ShowSettingsUI();
            }
        }
    }

    private void ShowSettingsUI()
    {
        settingsUI.Show();
        optionBar.Show();
        exitButton.Show();
        inventoryUI.Hide();
        characterUI.Hide();
        viewDeckButton.Hide();
    }

    private void HideSettingsUI()
    {
        settingsUI.Hide();
        optionBar.Hide();
        inventoryUI.Hide();
        characterUI.Hide();
        exitButton.Hide();
    }
}
