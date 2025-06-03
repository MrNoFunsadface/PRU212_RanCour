using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Menu
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private UIInventoryItem item;

        public void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
            mainCamera = Camera.main;
            item = GetComponentInChildren<UIInventoryItem>();
        }

        public void SetData(Sprite sprite, int quantity)
        {
            item.SetData(sprite, quantity);
        }

        private void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                Mouse.current.position.ReadValue(),
                mainCamera,
                out position);
            transform.position = canvas.transform.TransformPoint(position);
        }

        public void Toggle(bool value)
        {
            Debug.Log("Toggle MouseFollower: " + value);
            gameObject.SetActive(value);
        }
    }
}