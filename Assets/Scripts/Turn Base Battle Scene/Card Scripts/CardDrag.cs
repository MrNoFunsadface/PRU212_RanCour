using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 
// Summary:
//     CardDrag handles the drag-and-drop functionality for cards in the player's hand.
//     It manages the card's position, tilt, shadow, and parallax effects during dragging.
public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Fields and Properties

    [Header("Card Visuals")]
    [SerializeField] private RectTransform shadowTransform;
    [SerializeField] private RectTransform cardImage;
    [SerializeField] private RectTransform cardName;
    [SerializeField] private RectTransform costOutline;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    [HideInInspector] public Vector2 originalPosition;
    [HideInInspector] public CardSpawner cardSpawner;
    [HideInInspector] public Card card;

    private Coroutine returnCoroutine;
    private Coroutine parallaxResetCoroutine;
    private Coroutine rotationCoroutine;

    private Vector2 lastMousePosition;
    private Vector2 lastCardPosition;

    private Vector3 imageOriginalPos;
    private Vector3 nameOriginalPos;
    private Vector3 costOriginalPos;

    private const float TiltStrength = 4f;
    private const float MaxTilt = 40f;
    private const float SmoothDuration = .25f;
    private const float TiltLerpSpeed = 10f;
    private const float SpeedThreshold = 1f;
    private const string DropZoneTag = "DropZone";

    #endregion

    #region Methods

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        imageOriginalPos = cardImage.localPosition;
        nameOriginalPos = cardName.localPosition;
        costOriginalPos = costOutline.localPosition;
    }

    #region Drag Events Handlers
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        cardSpawner.OnCardBeginDrag(rectTransform);
        lastCardPosition = rectTransform.anchoredPosition;

        StopCoroutineIfRunning(ref rotationCoroutine);
        rotationCoroutine = StartCoroutine(SetRotation());

        StopCoroutineIfRunning(ref parallaxResetCoroutine);
        parallaxResetCoroutine = StartCoroutine(SetParallax());

        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        Vector2 delta = eventData.position - lastMousePosition;
        lastMousePosition = eventData.position;

        ApplyTilt(delta);
        ApplyShadow(delta);
        ApplyParallax(delta);

        UpdateOpacity(eventData);
        UpdateHealthBarState(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null || !eventData.pointerEnter.CompareTag(DropZoneTag))
        {
            // If the card is not dropped in a drop zone, return it to its original position
            StopCoroutineIfRunning(ref returnCoroutine);
            returnCoroutine = StartCoroutine(SmoothReturnToOriginalPosition());
        }
        else
        {
            // If the card is dropped in a drop zone
            if (eventData.pointerEnter.TryGetComponent<DropZoneScript>(out var info))
            {
                Debug.Log($"Dropped {card.cardName} on {info.enemyName} at order {info.enemyOrder}");
                UpdateBattleLog(info);

                DiscardCard();

                var targetEnemy = info.GetComponentInParent<EnemyStatus>();
                var reactionHandler = FindFirstObjectByType<ReactionHandler>();
                if (targetEnemy != null && reactionHandler != null)
                {
                    reactionHandler.OnCardDropped(card, targetEnemy);
                }
                else
                {
                    Debug.LogWarning("Missing EnemyStatus or ReactionHandler on drop target.");
                }
            }
            else
            {
                Debug.LogWarning("Cannot detect any drop zone info!");
            }

            // Set originalPosition to the first slot in hand
            int count = cardSpawner.cardRTList.Count;
            float angleStep = count > 1 ? (cardSpawner.maxAngle * 2f) / (count - 1) : 0f;
            originalPosition = cardSpawner.CalculateTargetPosition(0, angleStep);

            canvasGroup.blocksRaycasts = true;
        }

        StopCoroutineIfRunning(ref rotationCoroutine);
        StopCoroutineIfRunning(ref parallaxResetCoroutine);
    }

    #endregion

    #region Battle Logics

    private void UpdateBattleLog(DropZoneScript info)
    {
        var battleLog = FindFirstObjectByType<BattleLogScript>();
        if (battleLog != null)
        {
            battleLog.LogBattleEvent($"Dropped {card.cardName} on {info.enemyName} at order {info.enemyOrder}");
            battleLog.UpdateDisplayer();
        }
    }

    private void DiscardCard()
    {
        foreach (var dropZone in Object.FindObjectsByType<DropZoneScript>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            SetAlpha(dropZone.healthBar, 1f);
            SetAlpha(dropZone.activeHealthBar, 0f);
        }

        Deck.Instance.DiscardCard(card);
        Destroy(rectTransform.gameObject);
        return;
    }

    #endregion

    #region Card Movement and Effects

    private IEnumerator SmoothReturnToOriginalPosition()
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Quaternion startRotation = rectTransform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        float elapsedTime = 0f;
        while (elapsedTime < SmoothDuration)
        {
            float t = elapsedTime / SmoothDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, originalPosition, t);
            rectTransform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cardSpawner.OnCardEndDrag(rectTransform);
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator SetRotation()
    {
        while (true)
        {
            Vector2 currentPosition = rectTransform.anchoredPosition;
            float speed = (currentPosition - lastCardPosition).magnitude / Time.deltaTime;
            lastCardPosition = currentPosition;

            if (speed < SpeedThreshold)
            {
                StartCoroutine(SmoothRotateToIdentity());
            }

            yield return null;
        }
    }

    // Identity means x, y, z rotation are all 0
    private IEnumerator SmoothRotateToIdentity()
    {
        Quaternion startRotation = rectTransform.localRotation;
        Quaternion targetRotation = Quaternion.identity; // Reset to identity

        float elapsedTime = 0f;
        while (elapsedTime < SmoothDuration)
        {
            float t = elapsedTime / SmoothDuration;
            rectTransform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the rotation is exactly identity at the end
        rectTransform.localRotation = targetRotation;
    }

    private IEnumerator SetParallax()
    {
        float elapsedTime = 0f;

        // Store the current positions of the parallax elements
        Vector3 currentImagePos = cardImage.localPosition;
        Vector3 currentNamePos = cardName.localPosition;
        Vector3 currentCostPos = costOutline.localPosition;

        while (elapsedTime < SmoothDuration)
        {
            float t = elapsedTime / SmoothDuration;

            // Smoothly interpolate each element back to its original position
            cardImage.localPosition = Vector3.Lerp(currentImagePos, imageOriginalPos, t);
            cardName.localPosition = Vector3.Lerp(currentNamePos, nameOriginalPos, t);
            costOutline.localPosition = Vector3.Lerp(currentCostPos, costOriginalPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the elements are exactly at their original positions
        cardImage.localPosition = imageOriginalPos;
        cardName.localPosition = nameOriginalPos;
        costOutline.localPosition = costOriginalPos;
    }

    private void ApplyTilt(Vector2 delta)
    {
        float tiltX = Mathf.Clamp(-delta.y * TiltStrength, -MaxTilt, MaxTilt);
        float tiltY = Mathf.Clamp(delta.x * TiltStrength, -MaxTilt, MaxTilt);

        Quaternion targetTilt = Quaternion.Euler(tiltX, tiltY, 0);
        rectTransform.localRotation = Quaternion.Lerp(rectTransform.localRotation, targetTilt, Time.deltaTime * TiltLerpSpeed);
    }

    private void ApplyShadow(Vector2 delta)
    {
        Vector2 shadowOffset = new Vector2(-delta.x, -delta.y) * 0.5f;
        shadowTransform.anchoredPosition = Vector2.Lerp(shadowTransform.anchoredPosition, shadowOffset, Time.deltaTime * TiltLerpSpeed);
    }

    private void ApplyParallax(Vector2 delta)
    {
        float parallaxStrength = 0.5f;
        float maxOffset = 10f; // Maximum offset for parallax movement

        // Calculate new positions with parallax effect
        Vector3 newImagePos = imageOriginalPos + (Vector3)delta * parallaxStrength;
        Vector3 newNamePos = nameOriginalPos + (Vector3)delta * (parallaxStrength * 0.5f);
        Vector3 newCostPos = costOriginalPos + (Vector3)delta * (parallaxStrength * 0.7f);

        // Clamp the positions to ensure they stay within the card's bounds
        cardImage.localPosition = ClampPosition(newImagePos, imageOriginalPos, maxOffset);
        cardName.localPosition = ClampPosition(newNamePos, nameOriginalPos, maxOffset);
        costOutline.localPosition = ClampPosition(newCostPos, costOriginalPos, maxOffset);
    }

    private Vector3 ClampPosition(Vector3 currentPosition, Vector3 originalPos, float maxOffset)
    {
        return new Vector3(
            Mathf.Clamp(currentPosition.x, originalPos.x - maxOffset, originalPos.x + maxOffset),
            Mathf.Clamp(currentPosition.y, originalPos.y - maxOffset, originalPos.y + maxOffset),
            currentPosition.z // Keep the Z position unchanged
        );
    }

    private void UpdateOpacity(PointerEventData eventData)
    {
        float targetAlpha = (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag(DropZoneTag)) ? 0.5f : 1f;
        canvasGroup.alpha = targetAlpha;
    }

    private void UpdateHealthBarState(PointerEventData eventData)
    {
        // Default: show all normal health bars, hide all active health bars
        foreach (var dropZone in Object.FindObjectsByType<DropZoneScript>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            SetAlpha(dropZone.healthBar, 1f);
            SetAlpha(dropZone.activeHealthBar, 0f);
        }

        // If over a drop zone, show its active health bar, hide its normal health bar
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag(DropZoneTag))
        {
            if (eventData.pointerEnter.TryGetComponent<DropZoneScript>(out var dz))
            {
                SetAlpha(dz.healthBar, 0f);
                SetAlpha(dz.activeHealthBar, 1f);
            }
        }
    }

    private void SetAlpha(GameObject go, float alpha)
    {
        if (go == null) return;
        if (go.TryGetComponent<CanvasGroup>(out var cg)) cg.alpha = alpha;
    }

    private void StopCoroutineIfRunning(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    #endregion

    #endregion
}
