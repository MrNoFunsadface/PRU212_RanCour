using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Inventory
{
    public class OptionBar : MonoBehaviour
    {
        [SerializeField]
        private Button profileButton;

        [SerializeField]
        private Button inventoryButton;

        [SerializeField]
        private Button systemButton;

        public event Action<int> OnProfileSelected, OnInventorySelected, OnSettingsSelected;

        public void Awake()
        {
            Hide();
            if (profileButton != null)
                profileButton.onClick.AddListener(() => OnProfileSelected?.Invoke(0));
            if (inventoryButton != null)
                inventoryButton.onClick.AddListener(() => OnInventorySelected?.Invoke(0));
            if (systemButton != null)
                systemButton.onClick.AddListener(() => OnSettingsSelected?.Invoke(0));
        }

        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);
    }
}