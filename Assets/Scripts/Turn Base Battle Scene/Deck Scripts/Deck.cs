using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }


    [SerializeField] private CardCollection playerDeck;

    [SerializeField] private List<Card> deckPile = new();
    [SerializeField] private List<Card> discardPile = new();

    public List<Card> CardsToSpawn { get; private set; } = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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

    public List<Card> GetCardToSpawn()
    {
        // Shuffle the deck pile before drawing cards from it
        ShuffleDeckPile();
        DrawCard();

        return CardsToSpawn;
    }

    public void DrawCard(int amount = 5)
    {
        int amountToSpawn = Mathf.Min(amount, deckPile.Count);
        if (amountToSpawn <= 0) amountToSpawn = discardPile.Count;
        for (int i = 0; i < amountToSpawn; i++)
        {
            if (deckPile.Count <= 0)
            {
                deckPile = discardPile.ToList();
                discardPile.Clear();
                ShuffleDeckPile();
            }
            if (deckPile[0] == null) break;

            CardsToSpawn.Add(deckPile[0]);
            deckPile.RemoveAt(0);
        }
    }

    public void DiscardCard(Card card)
    {
        CardsToSpawn.Remove(card);
        discardPile.Add(card);
    }
}
