using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private CardCollection playerDeck;

    private List<Card> deckPile;
    private List<Card> discardPile;

    public void Shuffle()
    {
        if (deckPile == null || deckPile.Count == 0)
        {
            Debug.LogWarning("Deck is empty, cannot shuffle.");
            return;
        }
        for (int i = 0; i < deckPile.Count; i++)
        {
            Card temp = deckPile[i];
            int randomIndex = Random.Range(i, deckPile.Count);
            deckPile[i] = deckPile[randomIndex];
            deckPile[randomIndex] = temp;
        }
    }

    public void DrawCard(int amount = 5)
    {
        if (deckPile == null || deckPile.Count == 0)
        {
            Debug.LogWarning("Deck is empty, cannot draw a card.");
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            if (deckPile.Count <= 0)
            {
                discardPile = deckPile;
                discardPile.Clear();
                Shuffle();
            }
            if (deckPile.Count > 0)
            {
                Card drawnCard = deckPile[0];
                deckPile.RemoveAt(0);
            }
        }
    }

    public void DiscardCard()
    {
        if (discardPile == null)
        {
            discardPile = new List<Card>();
        }
    }
}
