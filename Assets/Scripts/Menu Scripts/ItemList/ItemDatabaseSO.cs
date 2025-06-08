using Scripts.Models;
using UnityEngine;

[CreateAssetMenu]
public class ItemDatabaseSO : ScriptableObject
{
    public ItemSO[] items;

    public ItemSO GetItemByName(string itemName)
    {
        foreach (ItemSO item in items)
        {
            if (item.ItemName == itemName)
            {
                return item;
            }
        }
        Debug.LogWarning($"Item with name {itemName} not found in database.");
        return null;
    }
}
