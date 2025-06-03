using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DropZoneFollower : MonoBehaviour
{
    [Header("Drop Zone Settings")]
    [SerializeField] private Transform enemy;
    [SerializeField] private Vector2 size; // Size of the drop zone
    [SerializeField] private Vector2 offset; // Offset from the enemy position

    private RectTransform rt; // RectTransform of the drop zone
    private Canvas canvas; // Canvas of the drop zone
    private RectTransform canvasRectTransform;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }
    }

    private void LateUpdate()
    {
        if (enemy == null || canvas == null || canvasRectTransform == null) return;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, enemy.position);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                screenPos,
                canvas.worldCamera,
                out Vector2 anchoredPos))
        {
            rt.anchoredPosition = anchoredPos + offset;
            rt.sizeDelta = size;
        }
    }
}
