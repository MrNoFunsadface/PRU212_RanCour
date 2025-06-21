using System.Collections.Generic;
using UnityEngine;

public class DeckViewSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<Card> cardsToSpawn;
    [SerializeField] private Transform content;

    public readonly List<RectTransform> cardRTList = new();

    public void SpawnCards(List<Card> cards)
    {
        cardsToSpawn = cards;
        if (cardsToSpawn == null || cardsToSpawn.Count == 0)
        {
            Debug.LogWarning("No cards to spawn. Please check the deck or card collection.");
            return;
        }

        Debug.Log($"Spawned {cardsToSpawn.Count} cards in deck view.");

        foreach (Card card in cardsToSpawn)
        {
            GameObject cardObj = Instantiate(cardPrefab, content);
            if (cardObj.TryGetComponent(out CardUI cardUI))
                cardUI.Setup(card);
            if (cardObj.TryGetComponent(out RectTransform cardRT))
            {
                cardRTList.Add(cardRT);
            }
        }
    }
}
