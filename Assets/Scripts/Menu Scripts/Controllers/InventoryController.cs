using Scripts.Models;
using Scripts.Menu;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Collections.Generic;
using Assets.Scripts.Inventory;

namespace Scripts.Controllers
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private MenuPage inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        [SerializeField]
        private OptionBar optionBar;

        [SerializeField]
        private ItemDatabaseSO itemDatabase;

        [SerializeField]
        private CharacterUI characterUI;

        [SerializeField]
        private ExitButton exitButton;

        [SerializeField]
        private ViewDeckButton viewDeckButton;

        [SerializeField]
        private SettingsUI settingsUI;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
            PrepareOptionBar();
            //InitializeMockData();
        }

        private void PrepareOptionBar()
        {
            optionBar.OnInventorySelected += HandleInventoryToggle;
        }

        private void HandleInventoryToggle(int obj)
        {
            ShowInventoryUI();
            UpdateAllInventoryUIItems();
        }

        private void InitializeMockData()
        {
            inventoryData.AddItem(itemDatabase.items[0], 1);
            inventoryData.AddItem(itemDatabase.items[1], 1);
            inventoryData.AddItem(itemDatabase.items[2], 1);
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryChanged += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.isEmpty) continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key,
                    item.Value.itemData.ItemSprite,
                    item.Value.quantity);
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

            inventoryUI.CreateDraggedItem(
                inventoryItem.itemData.ItemSprite,
                inventoryItem.quantity);
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

        public void Update()
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                Debug.Log("I key pressed, toggling inventory UI.");
                if (inventoryUI.gameObject.activeSelf)
                {
                    HideInventoryUI();
                }
                else
                {
                    ShowInventoryUI();
                    UpdateAllInventoryUIItems();
                }
            }
        }

        private void UpdateAllInventoryUIItems()
        {
            foreach (var item in inventoryData.GetCurrentInventoryState())
            {
                if (item.Value.itemData == null || item.Value.itemData.ItemSprite == null)
                {
                    Debug.LogWarning($"Item at key {item.Key} is not properly initialized.");
                    continue; // Skip this item
                }

                inventoryUI.UpdateData(item.Key, item.Value.itemData.ItemSprite, item.Value.quantity);
            }
        }

        private void HideInventoryUI()
        {
            inventoryUI.Hide();
            optionBar.Hide();
            exitButton.Hide();
            viewDeckButton.Hide();
        }

        private void ShowInventoryUI()
        {
            optionBar.Show();
            inventoryUI.Show();
            characterUI.Hide();
            exitButton.Show();
            settingsUI.Hide();
        }
    }
}