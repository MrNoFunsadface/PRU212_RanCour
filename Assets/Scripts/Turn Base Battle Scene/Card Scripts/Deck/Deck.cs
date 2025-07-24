using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }


    [SerializeField] private CardCollectionSO playerDeck;

    [SerializeField] private List<CardSO> deckPile = new();
    [SerializeField] private List<CardSO> discardPile = new();

    public List<CardSO> CardsToSpawn { get; private set; } = new();

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject); 

        SetUpDeck();
    }

    private void SetUpDeck()
    {
        for (int i = 0; i < playerDeck.CardsInCollection.Count; i++)
        {
            deckPile.Add(playerDeck.CardsInCollection[i]);
        }
    }

    public void ShuffleDeckPile()
    {
        for (int i = 0; i < deckPile.Count; i++)
        {
            var temp = deckPile[i];
            int randomIndex = Random.Range(i, deckPile.Count);
            deckPile[i] = deckPile[randomIndex];
            deckPile[randomIndex] = temp;
        }
    }

    public List<CardSO> GetCardToSpawn()
    {
        // Shuffle the deck pile before drawing cards from it
        ShuffleDeckPile();
        DrawCard();

        return CardsToSpawn;
    }

    public void DrawCard(int amount = 5)
    {
        int drawn = 0;
        while (drawn < amount)
        {
            // If deck is empty, try to reshuffle from discard
            if (deckPile.Count == 0)
            {
                if (discardPile.Count == 0)
                    break; // No more cards to draw

                deckPile = discardPile.ToList();
                discardPile.Clear();
                ShuffleDeckPile();
            }

            // If still no cards, break
            if (deckPile.Count == 0)
                break;

            CardsToSpawn.Add(deckPile[0]);
            deckPile.RemoveAt(0);
            drawn++;
        }
    }

    public void DiscardCard(CardSO card)
    {
        CardsToSpawn.Remove(card);
        discardPile.Add(card);
    }
}
