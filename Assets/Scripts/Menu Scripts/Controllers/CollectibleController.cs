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

    private EButton eButton;

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            eButton.Show();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            eButton.Hide();
        }
    }

    void Start()
    {
        eButton = FindFirstObjectByType<EButton>();
        if (eButton == null)
        {
            Debug.Log("[CollectibleController] EButton not found in the scene. Please ensure it is present for interaction.");
        }

        int collected = PlayerPrefs.GetInt(itemName + "_Collected");
        if (collected == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            var item = itemDatabase.GetItemByName(itemName);
            if (item == null)
            {
                Debug.LogWarning($"[CollectibleController] Item with name {itemName} not found in database.");
                return;
            }
            inventoryData.AddItem(item, 1);
            SoundManager.Instance.PlaySound(SoundEffectType.ITEMPICKUP);
            gameObject.SetActive(false);
            PlayerPrefs.SetInt(itemName + "_Collected", 1);
            PlayerPrefs.Save();

            if (EnvironmentController.Instance != null) EnvironmentController.Instance.UpdateEnvironment();
        }
    }
}
