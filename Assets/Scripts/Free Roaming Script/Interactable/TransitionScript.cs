using Scripts.Controllers;
using Scripts.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnPointId;

    private InventoryController inventoryController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (targetSceneName == string.Empty || targetSpawnPointId == string.Empty)
            {
                Debug.LogError("Target scene name or spawn point ID is not set in TransitionScript.");
                return;
            }

            if (targetSceneName == "Scene2")
            {
                if (inventoryController.CheckItemByName("MasterKey"))
                {
                    Debug.Log("Master Key is present, transitioning to Scene2.");
                    GameManager.Instance.useCustomSpawnPosition = true;
                    GameManager.Instance.targetSpawnPointId = targetSpawnPointId;
                    SceneManager.LoadScene(targetSceneName);
                }
                else
                {
                    Debug.Log("Master Key is required to enter Scene2. Transition aborted.");
                    return;
                }
            }

            GameManager.Instance.useCustomSpawnPosition = true;
            GameManager.Instance.targetSpawnPointId = targetSpawnPointId;
            SceneManager.LoadScene(targetSceneName);
        }
    }

    private void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
        if (inventoryController == null)
        {
            Debug.Log("InventoryController not found in the scene.");
        }
    }
}
