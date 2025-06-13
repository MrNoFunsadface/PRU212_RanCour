using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Models
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItem> inventoryItems;

        [SerializeField]
        private ItemDatabaseSO itemDatabase;

        [field: SerializeField]
        public int InventorySize { get; private set; } = 12;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryChanged;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>(InventorySize);
            for (int i = 0; i < InventorySize; i++)
            {
                if (PlayerPrefs.HasKey($"InventorySlot_{i}_Item"))
                {
                    Debug.Log($"Loading inventory slot {i} from PlayerPrefs.");
                    int itemId = PlayerPrefs.GetInt($"InventorySlot_{i}_Item");
                    int quantity = PlayerPrefs.GetInt($"InventorySlot_{i}_Quantity", 1);
                    ItemSO itemData = itemDatabase.GetItemByID(itemId);
                    inventoryItems.Add(new InventoryItem
                    {
                        itemData = itemData,
                        quantity = quantity
                    });
                }
                else inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public void AddItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    inventoryItems[i] = new InventoryItem
                    {
                        itemData = item,
                        quantity = quantity
                    };
                    PlayerPrefs.SetInt($"InventorySlot_{i}_Item", item.ID);
                    PlayerPrefs.SetInt($"InventorySlot_{i}_Quantity", quantity);
                    PlayerPrefs.Save();
                    break;
                }
            }
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue =
                new Dictionary<int, InventoryItem>();

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                    continue;
                returnValue.Add(i, inventoryItems[i]);
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.itemData, item.quantity);
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            InventoryItem item1 = inventoryItems[itemIndex1];
            inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
            inventoryItems[itemIndex2] = item1;
            PlayerPrefs.SetInt($"InventorySlot_{itemIndex1}_Item", item1.itemData?.ID ?? 0);
            PlayerPrefs.SetInt($"InventorySlot_{itemIndex1}_Quantity", item1.quantity);
            PlayerPrefs.SetInt($"InventorySlot_{itemIndex2}_Item", inventoryItems[itemIndex2].itemData?.ID ?? 0);
            PlayerPrefs.SetInt($"InventorySlot_{itemIndex2}_Quantity", inventoryItems[itemIndex2].quantity);
            PlayerPrefs.Save();
            InformAboutChange();
        }

        public bool CheckItemByName(string itemName)
        {
            return inventoryItems.Any(item => item.itemData != null && item.itemData.ItemName == itemName);
        }

        private void InformAboutChange()
        {
            OnInventoryChanged?.Invoke(GetCurrentInventoryState());
        }
    }

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSO itemData;
        public bool isEmpty => itemData == null;

        public InventoryItem changeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                itemData = itemData,
                quantity = newQuantity
            };
        }

        public static InventoryItem GetEmptyItem()
        =>
             new InventoryItem
             {
                 itemData = null,
                 quantity = 0,
             };

    }
}