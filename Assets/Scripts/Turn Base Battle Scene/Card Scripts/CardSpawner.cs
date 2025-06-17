using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
//
// Summary:
//     CardSpawner is responsible for spawning and managing cards in the player's hand. It handles the
//     positioning and fanning of cards, as well as drag-and-drop functionality for card interactions.

public class CardSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Card[] cardsToSpawn;
    [SerializeField] private Transform handCenter;
    [SerializeField] private float radius;
    private int dragIndex;

    public readonly List<RectTransform> cardRTList = new();
    public float maxAngle;

    private void Start()
    {
        SpawnAndFanCards();
    }

    private void SpawnAndFanCards()
    {
        if (cardsToSpawn == null || cardsToSpawn.Length == 0) return;

        float angleStep = cardsToSpawn.Length > 1 ? (maxAngle * 2f) / (cardsToSpawn.Length - 1) : 0f;

        for (int i = 0; i < cardsToSpawn.Length; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, handCenter);

            if (cardObj.TryGetComponent(out CardUI cardUI))
                cardUI.Setup(cardsToSpawn[i]);

            if (cardObj.TryGetComponent(out RectTransform cardRT))
            {
                PositionCard(cardRT, i, angleStep);
                cardRTList.Add(cardRT);

                if (cardObj.TryGetComponent(out CardDrag drag))
                {
                    drag.originalPosition = cardRT.anchoredPosition;
                    drag.cardSpawner = this;
                    drag.card = cardsToSpawn[i];
                }
            }
        }
    }

    public void OnCardBeginDrag(RectTransform card)
    {
        int idx = cardRTList.IndexOf(card);
        dragIndex = idx != -1 ? idx : 0;
        if (cardRTList.Remove(card))
            RepositionCards();
    }

    public void OnCardEndDrag(RectTransform card)
    {
        if (!cardRTList.Contains(card))
        {
            cardRTList.Insert(dragIndex, card);
            RepositionCards();
        }
    }

    private void RepositionCards()
    {
        int count = cardRTList.Count;
        if (count == 0) return;

        float angleStep = count > 1 ? (maxAngle * 2f) / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            StartCoroutine(SmoothReposition(cardRTList[i], i, angleStep));
        }
    }

    private IEnumerator SmoothReposition(RectTransform cardRT, int index, float angleStep)
    {
        float elapsedTime = 1f;
        float duration = 1.05f;
        Vector2 startPosition = cardRT.anchoredPosition;
        Quaternion startRotation = cardRT.localRotation;

        Vector2 targetPosition = CalculateTargetPosition(index, angleStep);
        Quaternion targetRotation = CalculateTargetRotation(index, angleStep);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            cardRT.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            cardRT.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cardRT.anchoredPosition = targetPosition;
        cardRT.localRotation = targetRotation;

        if (cardRT.TryGetComponent(out CardDrag drag))
            drag.originalPosition = cardRT.anchoredPosition;
    }

    public Vector2 CalculateTargetPosition(int index, float angleStep)
    {
        float angle = -maxAngle + (angleStep * index);
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)) * radius;
    }

    public Quaternion CalculateTargetRotation(int index, float angleStep)
    {
        float angle = -maxAngle + (angleStep * index);
        return Quaternion.Euler(0, 0, angle);
    }

    private void PositionCard(RectTransform cardRT, int index, float angleStep)
    {
        float angle = -maxAngle + (angleStep * index);
        float rad = angle * Mathf.Deg2Rad;
        Vector2 position = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)) * radius;
        cardRT.anchoredPosition = position;
        cardRT.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
