using System.Collections.Generic;
using UnityEngine;

//
// Summary:
//     CardCollection is a Collection of Card objects that can be used as a deck in the game.

[CreateAssetMenu(fileName = "New Card Collection", menuName = "Turn Base/Card/CardCollection", order = 3)]
public class CardCollectionSO : ScriptableObject
{
    public List<CardSO> CardsInCollection;

    public void AddCard(CardSO card)
    {
        if (CardsInCollection == null)
        {
            CardsInCollection = new List<CardSO>();
        }
        if (!CardsInCollection.Contains(card))
        {
            CardsInCollection.Add(card);
        }
    }

    public void RemoveCard(CardSO card)
    {
        if (CardsInCollection != null && CardsInCollection.Contains(card))
        {
            CardsInCollection.Remove(card);
        }
    }
}
