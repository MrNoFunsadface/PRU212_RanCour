using UnityEngine;
using UnityEngine.EventSystems;

public class UIResizeHandle : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] private RectTransform targetRect; // Assign your BattleLog panel here
    [SerializeField] private float minWidth = 500f;
    [SerializeField] private float minHeight = 320f;
    [SerializeField] private float maxWidth = 1000f;
    [SerializeField] private float maxHeight = 320f;
    [SerializeField] private bool isRightHandle = false; // Set this in Inspector for right handles

    private Vector2 originalSize;
    private Vector2 originalMousePosition;
    private Vector2 originalAnchoredPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (targetRect == null)
            targetRect = GetComponentInParent<RectTransform>();
        originalSize = targetRect.sizeDelta;
        originalAnchoredPosition = targetRect.anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRect, eventData.position, eventData.pressEventCamera, out originalMousePosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (targetRect == null) return;

        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRect, eventData.position, eventData.pressEventCamera, out localMousePosition);
        Vector2 offset = localMousePosition - originalMousePosition;

        float newWidth;
        float widthDelta;

        if (isRightHandle)
        {
            // For right-side handle: directly adjust width (positive offset = wider)
            newWidth = Mathf.Clamp(originalSize.x + offset.x, minWidth, maxWidth);
            widthDelta = newWidth - originalSize.x;
            
            // Move the right edge, keep the left edge fixed
            targetRect.sizeDelta = new Vector2(newWidth, Mathf.Clamp(originalSize.y + offset.y, minHeight, maxHeight));
            targetRect.anchoredPosition = originalAnchoredPosition + new Vector2(widthDelta * 0.5f, 0);
        }
        else
        {
            // For left-side handle: inverse adjust width (negative offset = wider)
            newWidth = Mathf.Clamp(originalSize.x - offset.x, minWidth, maxWidth);
            widthDelta = newWidth - originalSize.x;
            
            // Move the left edge, keep the right edge fixed
            targetRect.sizeDelta = new Vector2(newWidth, Mathf.Clamp(originalSize.y + offset.y, minHeight, maxHeight));
            targetRect.anchoredPosition = originalAnchoredPosition + new Vector2(widthDelta * 0.5f, 0);
        }
    }
}
