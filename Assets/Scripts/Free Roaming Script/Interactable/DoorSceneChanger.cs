using Scripts.Controllers;
using Scripts.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSceneChanger : MonoBehaviour
{
    [Header("Target Scene and Spawn Point")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnPointId;

    [Header("Requirements")]
    [SerializeField] private bool requiresMasterKey = false;

    [Header("Transition Settings")]
    [SerializeField] private float startAnimationTime = 1.0f;
    [SerializeField] private string startTrigger;

    private InventoryController inventoryController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (targetSceneName == string.Empty)
            {
                Debug.LogError("[DoorSceneChanger] Target scene name is empty.");
                return;
            }

            if (targetSpawnPointId == string.Empty)
            {
                Debug.LogError("[DoorSceneChanger] Target spawn point ID is empty.");
                return;
            }

            if (requiresMasterKey)
            {
                Debug.Log($"[DoorSceneChanger] Master Key is required to go to {targetSceneName}.");
                if (inventoryController.CheckItemByName("MasterKey"))
                {
                    Debug.Log($"[DoorSceneChanger] Master Key is present, transitioning to {targetSceneName}");
                    GameManager.Instance.useCustomSpawnPosition = true;
                    GameManager.Instance.targetSpawnPointId = targetSpawnPointId;
                    LevelLoader.Instance.LoadLevel(targetSceneName, startTrigger, startAnimationTime);
                }
                else
                {
                    Debug.Log($"[DoorSceneChanger] Master Key is required to enter {targetSceneName}. Transition aborted.");
                    return;
                }
            }

            GameManager.Instance.useCustomSpawnPosition = true;
            GameManager.Instance.targetSpawnPointId = targetSpawnPointId;
            LevelLoader.Instance.LoadLevel(targetSceneName, startTrigger, startAnimationTime);
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
