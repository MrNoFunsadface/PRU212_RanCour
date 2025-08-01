using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleLog : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static BattleLog Instance { get; private set; }
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup scrollViewCanvasGroup;
    [SerializeField] private List<string> battleLogText = new();
    [SerializeField] private TMP_Text displayer;


    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        scrollViewCanvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // Clamp to canvas bounds
        Vector2 minPosition = Vector2.zero;
        Vector2 maxPosition = Vector2.zero;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 panelSize = rectTransform.rect.size;
        Vector2 canvasSize = canvasRect.rect.size;

        // Calculate min/max so the panel stays fully inside the canvas
        minPosition.x = -canvasSize.x * 0.5f + panelSize.x * rectTransform.pivot.x;
        maxPosition.x = canvasSize.x * 0.5f - panelSize.x * (1 - rectTransform.pivot.x);
        minPosition.y = -canvasSize.y * 0.5f + panelSize.y * rectTransform.pivot.y;
        maxPosition.y = canvasSize.y * 0.5f - panelSize.y * (1 - rectTransform.pivot.y);

        Vector2 clampedPosition = rectTransform.anchoredPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minPosition.x, maxPosition.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minPosition.y, maxPosition.y);

        rectTransform.anchoredPosition = clampedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void LogBattleEvent(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;
        battleLogText.Insert(0, message); // Add to the top of the list
    }

    public void UpdateDisplayer()
    {
        displayer.text = string.Empty; // Clear the displayer text
        scrollViewCanvasGroup.blocksRaycasts = true;
        foreach (string text in battleLogText)
        {
            if (!string.IsNullOrEmpty(text))
            {
                displayer.text += text + "\n\n"; // Append each log message
            }
        }
    }
}