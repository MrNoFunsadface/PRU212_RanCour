using NUnit.Framework;
using Scripts.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Menu
{
    public class MenuPage : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem itemSlotRefab;

        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private UIInventoryDescription DescriptionUI;

        [SerializeField]
        private MouseFollower mouseFollower;

        [SerializeField]
        private InventorySO inventoryData;

        [SerializeField]
        private ViewDeckButton viewDeckButton;

        [SerializeField]
        private BookToCardCollection bookToCardCollection;

        [SerializeField]
        private DeckViewSpawner deckViewSpawner;

        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

        private int currentInvSlot = -1;

        public event Action<int> OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging;

        public event Action<int, int> OnSwapItems;

        private void Awake()
        {
            Hide();
            DescriptionUI.ResetDescription();
            mouseFollower.Toggle(false);
        }

        public void UpdateData(int itemIndex,
            Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemSlotRefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel, false);
                listOfUIItems.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDropped += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
                //Debug.Log($"Initialized UI item at index {i}");
            }
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            Debug.Log("Item selected: " + item.name);
            int index = listOfUIItems.IndexOf(item);
            if (index == -1) return;
            OnDescriptionRequested?.Invoke(index);

            var inventoryItem = inventoryData.GetItemAt(index);
            if (!inventoryItem.isEmpty)
            {
                if (inventoryData.GetItemAt(index).itemData.name == "AlchemyBookVolume1")
                {
                    viewDeckButton.Show();
                    var Deck = bookToCardCollection.GetCollectionForBook(inventoryData.GetItemAt(index).itemData);
                    deckViewSpawner.SpawnCards(Deck.CardsInCollection);
                }
                else viewDeckButton.Hide();
            } else viewDeckButton.Hide();

            item.Select();
        }

        private void HandleBeginDrag(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1) return;
            currentInvSlot = index;
            HandleItemSelection(item);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleSwap(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1)
            {
                return;
            }
            OnSwapItems?.Invoke(currentInvSlot, index);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentInvSlot = -1;
        }

        private void HandleEndDrag(UIInventoryItem item)
        {
            ResetDraggedItem();
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            DescriptionUI.ResetDescription();
            ResetSelection();
        }

        public void ResetSelection()
        {
            DescriptionUI.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        public void UpdateDescription(int itemIndex, Sprite sprite, int atkStats
            , int defStats, string itemName, string itemDescription
            , string quirk, string quirktDescription)
        {
            DescriptionUI.SetDescription(sprite, atkStats, defStats, itemName, itemDescription, quirk, quirktDescription);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        internal void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}