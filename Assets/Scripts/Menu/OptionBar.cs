using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class OptionBar : MonoBehaviour
    {
        [SerializeField]
        private Button profileButton;

        [SerializeField]
        private Button inventoryButton;

        [SerializeField]
        private Button systemButton;

        public event Action<int> OnProfileSelected, OnInventorySelected;

        public void Awake()
        {
            Hide();
            if (profileButton != null)
                profileButton.onClick.AddListener(() => OnProfileSelected?.Invoke(0));
            if (inventoryButton != null)
                inventoryButton.onClick.AddListener(() => OnInventorySelected?.Invoke(0));
        }

        public void Hide() => gameObject.SetActive(false);
        
        public void Show() => gameObject.SetActive(true);
    }
}