using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class UIInventoryItem : MonoBehaviour
    {
        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TMP_Text quantityTxt;

        [SerializeField]
        private Image borderImage;

        public event Action<UIInventoryItem> OnItemClicked,
            OnItemDropped, OnItemBeginDrag, OnItemEndDrag,
            OnRightMouseBtnClick;

        private bool empty = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            if (itemImage != null && itemImage.gameObject != null)
                itemImage.gameObject.SetActive(false);
            empty = true;
        }

        public void Deselect()
        {
            if (borderImage != null && borderImage.gameObject != null)
                borderImage.gameObject.SetActive(false);
        }

        public void SetData(Sprite sprite, int quanity)
        {
            if (itemImage != null && itemImage.gameObject != null)
            {
                itemImage.gameObject.SetActive(true);
                itemImage.sprite = sprite;
            }
            if (quantityTxt != null)
            {
                quantityTxt.text = quanity + "";
            }
            empty = false;
        }

        public void Select()
        {
            borderImage.gameObject.SetActive(true);
        }

        public void OnBeginDrag()
        {
            if (empty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnDrop()
        {
            OnItemDropped?.Invoke(this);
        }

        public void OnEndDrag()
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnPointerClick(BaseEventData baseEventData)
        {
            PointerEventData pointerEventData = (PointerEventData)baseEventData;
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                OnItemClicked?.Invoke(this);
            }
            else if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
        }
    }
}