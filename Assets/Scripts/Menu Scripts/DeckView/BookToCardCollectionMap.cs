using Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookToCardCollection", menuName = "Menu/BookToCardCollection")]
public class BookToCardCollection : ScriptableObject
{
    [System.Serializable]
    public struct BookCollectionPair
    {
        public ItemSO book;
        public CardCollectionSO collection;
    }

    public List<BookCollectionPair> mappings;

    public CardCollectionSO GetCollectionForBook(ItemSO book)
    {
        foreach (var pair in mappings)
        {
            if (pair.book == book)
                return pair.collection;
        }
        return null;
    }
}
