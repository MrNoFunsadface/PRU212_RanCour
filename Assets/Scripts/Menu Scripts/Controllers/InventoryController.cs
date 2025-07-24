using Scripts.Models;
using Scripts.Menu;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Assets.Scripts.Inventory;

namespace Scripts.Controllers
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private MenuPage inventoryUI;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private OptionBar optionBar;
        [SerializeField] private ItemDatabaseSO itemDatabase;
        [SerializeField] private CharacterUI characterUI;
        [SerializeField] private ExitButton exitButton;
        [SerializeField] private ViewDeckButton viewDeckButton;
        [SerializeField] private SettingsUI settingsUI;

        public List<InventoryItem> initialItems = new();

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
            PrepareOptionBar();
        }

        private void PrepareOptionBar()
        {
            optionBar.OnInventorySelected += HandleInventoryToggle;
            optionBar.OnProfileSelected += HandleProfileSelected;
            optionBar.OnSettingsSelected += HandleSettingsSelected;
        }

        private void HandleSettingsSelected(int obj)
        {
            ShowSettingsUI();
            SoundManager.PlaySound(SoundEffectType.BUTTONCLICK);
        }

        private void HandleProfileSelected(int obj)
        {
            ShowCharacterUI();
            SoundManager.PlaySound(SoundEffectType.BUTTONCLICK);
        }

        private void HandleInventoryToggle(int obj)
        {
            ShowInventoryUI();
            UpdateAllInventoryUIItems();
            SoundManager.PlaySound(SoundEffectType.BUTTONCLICK);
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryChanged += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (!item.isEmpty)
                    inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.itemData.ItemSprite, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.InventorySize);
            inventoryUI.OnDescriptionRequested += HandleDescription;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleStartDragging;
            inventoryUI.OnItemActionRequested += HandleItemAction;
        }

        private void HandleItemAction(int itemIndex)
        {
            throw new NotImplementedException();
        }

        private void HandleStartDragging(int itemIndex)
        {
            inventoryUI.ResetSelection();
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty) return;
            inventoryUI.CreateDraggedItem(inventoryItem.itemData.ItemSprite, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        private void HandleDescription(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            inventoryUI.UpdateDescription(itemIndex,
                inventoryItem.itemData.ItemSprite,
                inventoryItem.itemData.AttackStats,
                inventoryItem.itemData.DefenseStats,
                inventoryItem.itemData.ItemName,
                inventoryItem.itemData.ItemDescription,
                inventoryItem.itemData.Quirk,
                inventoryItem.itemData.QuirkDescription);
        }

        public bool CheckItemByName(string itemName)
        {
            return inventoryData.CheckItemByName(itemName);
        }

        private void Update()
        {
            bool inventoryActive = inventoryUI.gameObject.activeSelf;
            bool characterActive = characterUI.gameObject.activeSelf;
            bool settingsActive = settingsUI.gameObject.activeSelf;

            // ESC: open inventory if all closed, close any open menu if any open
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (!inventoryActive && !characterActive && !settingsActive)
                {
                    ShowInventoryUI();
                    UpdateAllInventoryUIItems();
                }
                else
                {
                    HideAllMenus();
                }
                return;
            }

            // I: toggle inventory, switch from other menus, close inventory
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                if (!inventoryActive)
                {
                    HideAllMenus();
                    ShowInventoryUI();
                    UpdateAllInventoryUIItems();
                }
                else
                {
                    HideAllMenus();
                }
                return;
            }

            // C: toggle character, switch from other menus, close character
            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                if (!characterActive)
                {
                    HideAllMenus();
                    ShowCharacterUI();
                }
                else
                {
                    HideAllMenus();
                }
                return;
            }

            // P: toggle settings, switch from other menus, close settings
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                if (!settingsActive)
                {
                    HideAllMenus();
                    ShowSettingsUI();
                }
                else
                {
                    HideAllMenus();
                }
                return;
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
            SoundManager.PlaySound(SoundEffectType.MENUOPEN);
        }

        private void ShowCharacterUI()
        {
            characterUI.Show();
            optionBar.Show();
            exitButton.Show();
            inventoryUI.Hide();
            settingsUI.Hide();
            viewDeckButton.Hide();
            SoundManager.PlaySound(SoundEffectType.MENUOPEN);
        }

        private void HideAllMenus()
        {
            inventoryUI.Hide();
            optionBar.Hide();
            exitButton.Hide();
            viewDeckButton.Hide();
            characterUI.Hide();
            settingsUI.Hide();
            SoundManager.PlaySound(SoundEffectType.MENUOPEN);
        }

        private void ShowInventoryUI()
        {
            optionBar.Show();
            inventoryUI.Show();
            characterUI.Hide();
            exitButton.Show();
            settingsUI.Hide();
            viewDeckButton.Hide();
            SoundManager.PlaySound(SoundEffectType.MENUOPEN);
        }

        private void UpdateAllInventoryUIItems()
        {
            foreach (var item in inventoryData.GetCurrentInventoryState())
            {
                if (item.Value.itemData == null || item.Value.itemData.ItemSprite == null)
                {
                    Debug.LogWarning($"Item at key {item.Key} is not properly initialized.");
                    continue;
                }
                inventoryUI.UpdateData(item.Key, item.Value.itemData.ItemSprite, item.Value.quantity);
            }
        }

        public void OpenMenu()
        {
            SoundManager.PlaySound(SoundEffectType.BUTTONCLICK);
            if (!inventoryUI.gameObject.activeSelf)
            {
                HideAllMenus();
                ShowInventoryUI();  
                UpdateAllInventoryUIItems();
            }
            else
            {
                HideAllMenus();
            }
            return;
        }
    }
}