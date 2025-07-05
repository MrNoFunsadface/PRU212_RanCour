using Scripts.Models;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabaseSO", menuName = "Menu/ItemDatabaseSO")]
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

    public ItemSO GetItemByID(int itemID)
    {
        foreach (ItemSO item in items)
        {
            if (item.ID == itemID)
            {
                return item;
            }
        }
        Debug.LogWarning($"Item with ID {itemID} not found in database.");
        return null;
    }
}
