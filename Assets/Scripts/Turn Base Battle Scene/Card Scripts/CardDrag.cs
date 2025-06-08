using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    [HideInInspector] public Vector2 originalPosition;
    [HideInInspector] public CardSpawner cardSpawner;

    private Coroutine returnCoroutine;
    private Coroutine rotationCoroutine;
    private Coroutine tiltResetCoroutine;
    private Coroutine paralaxResetCoroutine;
    private Coroutine speedTrackingCoroutine;

    private Vector2 lastMousePosition;
    private Vector2 lastCardPosition;

    private const float TiltStrength = 6f;
    private const float MaxTilt = 40f;
    private const float SmoothDuration = .25f;
    private const float TiltLerpSpeed = 10f;
    private const float SpeedThreshold = 1f;

    [SerializeField] private RectTransform shadowTransform;
    [SerializeField] private RectTransform cardImage;
    [SerializeField] private RectTransform cardName;
    [SerializeField] private RectTransform cardDescription;
    [SerializeField] private RectTransform costOutline;

    private Vector3 imageOriginalPos;
    private Vector3 nameOriginalPos;
    private Vector3 descOriginalPos;
    private Vector3 costOriginalPos;

    private float speed;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        imageOriginalPos = cardImage.localPosition;
        nameOriginalPos = cardName.localPosition;
        descOriginalPos = cardDescription.localPosition;
        costOriginalPos = costOutline.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        cardSpawner.OnCardBeginDrag(rectTransform);
        lastCardPosition = rectTransform.anchoredPosition;

        StopCoroutineIfRunning(ref rotationCoroutine);
        rotationCoroutine = StartCoroutine(SmoothRotateTo(Vector3.zero));

        StopCoroutineIfRunning(ref speedTrackingCoroutine);
        speedTrackingCoroutine = StartCoroutine(TrackSpeed());

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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null || !eventData.pointerEnter.CompareTag("DropZone"))
        {
            StopCoroutineIfRunning(ref returnCoroutine);
            returnCoroutine = StartCoroutine(SmoothReturnToOriginalPosition());
        }
        else
        {
            var info = eventData.pointerEnter.GetComponent<DropZoneScript>();
            if (info != null)
            {
                Debug.Log($"Dropped a card on {info.enemyName} at order {info.enemyOrder}");
            }
            else
            {
                Debug.Log("Cannot detect any drop zone info");
            }
            canvasGroup.blocksRaycasts = true;

            var info = eventData.pointerEnter.GetComponent<DropZoneScript>();
            Debug.Log(info);
            if (info != null)
            {
                Debug.Log($"Dropped a card on {info.enemyName} at order {info.enemyOrder}");
            }
            else
            {
                Debug.Log("Cannot detect any drop zone info");
            }
        }

        StopCoroutineIfRunning(ref speedTrackingCoroutine);

        if (speed < SpeedThreshold)
        {
            ResetTilt();
        }

        StopCoroutineIfRunning(ref tiltResetCoroutine);
        tiltResetCoroutine = StartCoroutine(SmoothRotateTo(Vector3.zero));

        StopCoroutineIfRunning(ref paralaxResetCoroutine);
        paralaxResetCoroutine = StartCoroutine(ResetParallax());
    }

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

    private IEnumerator SmoothRotateTo(Vector3 targetEulerAngles)
    {
        Quaternion startRotation = rectTransform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        float elapsedTime = 0f;
        while (elapsedTime < SmoothDuration)
        {
            float t = elapsedTime / SmoothDuration;
            rectTransform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localRotation = targetRotation;
        shadowTransform.anchoredPosition = Vector2.zero;
    }

    private IEnumerator TrackSpeed()
    {
        while (true)
        {
            Vector2 currentPosition = rectTransform.anchoredPosition;
            speed = (currentPosition - lastCardPosition).magnitude / Time.deltaTime;
            lastCardPosition = currentPosition;

            if (speed < SpeedThreshold)
            {
                ResetTilt();
            }

            yield return null;
        }
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
        float parallaxStrength = 1f;
        float maxOffset = 10f; // Maximum offset for parallax movement

        // Calculate new positions with parallax effect
        Vector3 newImagePos = imageOriginalPos + new Vector3(delta.x, delta.y, 0) * parallaxStrength;
        Vector3 newNamePos = nameOriginalPos + new Vector3(delta.x, delta.y, 0) * (parallaxStrength * 0.5f);
        Vector3 newDescPos = descOriginalPos + new Vector3(delta.x, delta.y, 0) * (parallaxStrength * 0.3f);
        Vector3 newCostPos = costOriginalPos + new Vector3(delta.x, delta.y, 0) * (parallaxStrength * 0.7f);

        // Clamp the positions to ensure they stay within the card's bounds
        cardImage.localPosition = ClampPosition(newImagePos, imageOriginalPos, maxOffset);
        cardName.localPosition = ClampPosition(newNamePos, nameOriginalPos, maxOffset);
        cardDescription.localPosition = ClampPosition(newDescPos, descOriginalPos, maxOffset);
        costOutline.localPosition = ClampPosition(newCostPos, costOriginalPos, maxOffset);
    }

    private Vector3 ClampPosition(Vector3 currentPosition, Vector3 originalPosition, float maxOffset)
    {
        return new Vector3(
            Mathf.Clamp(currentPosition.x, originalPosition.x - maxOffset, originalPosition.x + maxOffset),
            Mathf.Clamp(currentPosition.y, originalPosition.y - maxOffset, originalPosition.y + maxOffset),
            currentPosition.z // Keep the Z position unchanged
        );
    }

    private void UpdateOpacity(PointerEventData eventData)
    {
        float targetAlpha = (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("DropZone")) ? 0.5f : 1f;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * TiltLerpSpeed);
    }

    private void StopCoroutineIfRunning(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    private IEnumerator ResetParallax()
    {
        float elapsedTime = 0f;

        // Store the current positions of the parallax elements
        Vector3 currentImagePos = cardImage.localPosition;
        Vector3 currentNamePos = cardName.localPosition;
        Vector3 currentDescPos = cardDescription.localPosition;
        Vector3 currentCostPos = costOutline.localPosition;

        while (elapsedTime < SmoothDuration)
        {
            float t = elapsedTime / SmoothDuration;

            // Smoothly interpolate each element back to its original position
            cardImage.localPosition = Vector3.Lerp(currentImagePos, imageOriginalPos, t);
            cardName.localPosition = Vector3.Lerp(currentNamePos, nameOriginalPos, t);
            cardDescription.localPosition = Vector3.Lerp(currentDescPos, descOriginalPos, t);
            costOutline.localPosition = Vector3.Lerp(currentCostPos, costOriginalPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the elements are exactly at their original positions
        cardImage.localPosition = imageOriginalPos;
        cardName.localPosition = nameOriginalPos;
        cardDescription.localPosition = descOriginalPos;
        costOutline.localPosition = costOriginalPos;
    }

    private void ResetTilt()
    {
        // Stop any ongoing tilt reset coroutine
        StopCoroutineIfRunning(ref tiltResetCoroutine);

        // Start a new coroutine to smoothly rotate to identity
        tiltResetCoroutine = StartCoroutine(SmoothRotateToIdentity());
    }

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
}
