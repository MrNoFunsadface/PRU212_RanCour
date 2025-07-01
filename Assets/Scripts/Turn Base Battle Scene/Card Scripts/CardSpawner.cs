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
    [SerializeField] private List<Card> cardsToSpawn;
    [SerializeField] private Transform handCenter;
    [SerializeField] private float radius;
    private int dragIndex;

    public readonly List<RectTransform> cardRTList = new();
    public float maxAngle;

    private void Start()
    {
        SpawnAndFanCards();
    }

    public void SpawnAndFanCards()
    {
        cardsToSpawn = Deck.Instance.GetCardToSpawn();
        if (cardsToSpawn == null || cardsToSpawn.Count == 0)
        {
            Debug.LogWarning("No cards to spawn. Please check the deck or card collection.");
            return;
        }

        Debug.Log($"Spawned {cardsToSpawn.Count} cards in hand.");

        for (int i = 0; i < cardsToSpawn.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, handCenter);

            if (cardObj.TryGetComponent(out CardUI cardUI))
                cardUI.Setup(cardsToSpawn[i]);

            if (cardObj.TryGetComponent(out RectTransform cardRT))
            {
                cardRTList.Add(cardRT);
                if (cardObj.TryGetComponent(out CardDrag drag))
                {
                    drag.originalPosition = cardRT.anchoredPosition;
                    drag.cardSpawner = this;
                    drag.card = cardsToSpawn[i];
                }
            }
        }
        cardsToSpawn.Clear();
        RepositionCards(0f, 0.75f);
    }

    public void OnCardBeginDrag(RectTransform card)
    {
        int idx = cardRTList.IndexOf(card);
        dragIndex = idx != -1 ? idx : 0;
        if (cardRTList.Remove(card)) RepositionCards(1f, 1.05f);
    }

    public void OnCardEndDrag(RectTransform card)
    {
        if (!cardRTList.Contains(card))
        {
            cardRTList.Insert(dragIndex, card);
            RepositionCards(1f, 1.05f);
        }
    }

    private void RepositionCards(float elapsedTime, float duration)
    {
        int count = cardRTList.Count;
        if (count == 0) return;

        float angleStep = count > 1 ? (maxAngle * 2f) / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            StartCoroutine(SmoothReposition(cardRTList[i], i, angleStep, elapsedTime, duration));
        }
    }

    // in CardSpawner.cs, inside SmoothReposition:
    private IEnumerator SmoothReposition(RectTransform cardRT, int index, float angleStep, float elapsedTime, float duration)
    {
        // EXIT if this card was destroyed
        if (cardRT == null)
            yield break;

        Vector2 startPosition = cardRT.anchoredPosition;
        Quaternion startRotation = cardRT.localRotation;
        Vector2 targetPosition = CalculateTargetPosition(index, angleStep);
        Quaternion targetRotation = CalculateTargetRotation(index, angleStep);

        while (elapsedTime < duration)
        {
            // guard each loop in case something got destroyed partway
            if (cardRT == null)
                yield break;

            float t = elapsedTime / duration;
            cardRT.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            cardRT.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // final snap
        if (cardRT != null)
        {
            cardRT.anchoredPosition = targetPosition;
            cardRT.localRotation = targetRotation;
        }
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
}
