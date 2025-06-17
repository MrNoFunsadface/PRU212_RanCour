using Assets.Scripts.Inventory;
using Scripts.Menu;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterUI characterUI;

    [SerializeField]
    private OptionBar OptionBar;

    [SerializeField]
    private MenuPage InventoryUI;

    [SerializeField]
    private ExitButton ExitButton;

    [SerializeField]
    private SettingsUI settingUI;

    [SerializeField]
    private ViewDeckButton viewDeckButton;

    void Awake()
    {
        PrepareOptionBar();
    }

    private void PrepareOptionBar()
    {
        OptionBar.OnProfileSelected += HandleProfileSelected;
    }

    private void HandleProfileSelected(int obj)
    {
        ShowCharacterUI();
    }

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            Debug.Log("C key pressed, toggling inventory UI.");
            if (characterUI.gameObject.activeSelf)
            {
                HideCharacterUI();
            }
            else
            {
                ShowCharacterUI();
            }
        }
    }

    private void HideCharacterUI()
    {
        characterUI.Hide();
        OptionBar.Hide();
        ExitButton.Hide();
        InventoryUI.Hide();
        settingUI.Hide();
        viewDeckButton.Hide();
    }

    private void ShowCharacterUI()
    {
        characterUI.Show();
        OptionBar.Show();
        InventoryUI.Hide();
        ExitButton.Show();
        settingUI.Hide();
        viewDeckButton.Hide();
    }
}
