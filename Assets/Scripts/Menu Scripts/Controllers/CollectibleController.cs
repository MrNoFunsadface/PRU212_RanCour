using Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectibleController : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private ItemDatabaseSO itemDatabase;

    [SerializeField]
    private EButton eButton;

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            eButton.Show();
            Debug.Log("player steps in " + itemName + "'s hitbox");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            eButton.Hide();
            Debug.Log("player steps out " + itemName + "'s hitbox");
        }
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            var item = itemDatabase.GetItemByName(itemName);
            if (item == null)
            {
                Debug.LogWarning($"Item with name {itemName} not found in database.");
                return;
            }
            inventoryData.AddItem(item, 1);
            gameObject.SetActive(false);
            Debug.Log($"Added {itemName} to inventory.");
        }
    }
}
