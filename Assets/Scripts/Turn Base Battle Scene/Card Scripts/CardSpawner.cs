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
    [SerializeField] private CardSO[] cardsToSpawn;
    [SerializeField] private Transform handCenter;
    [SerializeField] private float radius;
    [SerializeField] private float maxAngle;

    public Coroutine repositionCard;

    private readonly List<RectTransform> cardInstances = new List<RectTransform>();

    private void Start()
    {
        SpawnAndFanCards();
    }

    private void SpawnAndFanCards()
    {
        if (cardsToSpawn == null || cardsToSpawn.Length == 0) return;

        float angleStep = cardsToSpawn.Length > 1 ? (maxAngle * 2) / (cardsToSpawn.Length - 1) : 0;

        for (int i = 0; i < cardsToSpawn.Length; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, handCenter);

            if (cardObj.TryGetComponent(out CardUI cardUI))
            {
                cardUI.Setup(cardsToSpawn[i]);
            }

            if (cardObj.TryGetComponent(out RectTransform cardRT))
            {
                PositionCard(cardRT, i, angleStep);
                cardInstances.Add(cardRT);
            }

            if (cardObj.TryGetComponent(out CardDrag drag))
            {
                drag.originalPosition = cardRT.anchoredPosition;
                drag.cardSpawner = this; // Assign reference to CardSpawner
            }
        }
    }

    private int index;

    public void OnCardBeginDrag(RectTransform card)
    {
        index = cardInstances.IndexOf(card) != -1 ? cardInstances.IndexOf(card) : 0;
        if (cardInstances.Remove(card))
        {
            RepositionCards();
        }
    }

    public void OnCardEndDrag(RectTransform card)
    {
        if (!cardInstances.Contains(card))
        {
            cardInstances.Insert(index, card);
            RepositionCards();
        }
    }

    private void RepositionCards()
    {
        if (cardInstances.Count == 0) return;

        float angleStep = cardInstances.Count > 1 ? (maxAngle * 2) / (cardInstances.Count - 1) : 0;

        for (int i = 0; i < cardInstances.Count; i++)
        {
            repositionCard = StartCoroutine(SmoothReposition(cardInstances[i], i, angleStep));
        }
    }

    private IEnumerator SmoothReposition(RectTransform cardRT, int index, float angleStep)
    {
        float elapsedTime = 1f; // Time elapsed since the start of the animation
        float duration = 1.05f; // Duration of the return animation
        Vector2 startPosition = cardRT.anchoredPosition;
        Quaternion startRotation = cardRT.localRotation;

        // Calculate the target position and rotation where the card should go into
        float angle = -maxAngle + (angleStep * index);
        float rad = angle * Mathf.Deg2Rad;
        Vector2 targetPosition = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)) * radius;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        while (elapsedTime < duration)
        {
            cardRT.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            cardRT.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
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
