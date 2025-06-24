using Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BookToCardCollection : ScriptableObject
{
    [System.Serializable]
    public struct BookCollectionPair
    {
        public ItemSO book;
        public CardCollection collection;
    }

    public List<BookCollectionPair> mappings;

    public CardCollection GetCollectionForBook(ItemSO book)
    {
        foreach (var pair in mappings)
        {
            if (pair.book == book)
                return pair.collection;
        }
        return null;
    }
}
