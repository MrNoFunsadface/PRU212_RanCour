using System.Collections.Generic;
using UnityEngine;

//
// Summary:
//     CardCollection is a Collection of Card objects that can be used as a deck in the game.

[CreateAssetMenu(fileName = "New Card Collection", menuName = "Turn Base/CardCollection", order = 3)]
public class CardCollection : ScriptableObject
{
    public List<Card> CardsInCollection;

    public void AddCard(Card card)
    {
        if (CardsInCollection == null)
        {
            CardsInCollection = new List<Card>();
        }
        if (!CardsInCollection.Contains(card))
        {
            CardsInCollection.Add(card);
        }
    }

    public void RemoveCard(Card card)
    {
        if (CardsInCollection != null && CardsInCollection.Contains(card))
        {
            CardsInCollection.Remove(card);
        }
    }
}
