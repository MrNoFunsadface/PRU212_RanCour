using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    private FogBarrel fogBarrel;
    private EnemyAI[] enemies;

    [SerializeField] private bool debugMode = true;

    // Delay initialization to ensure all objects are properly loaded
    private void Start()
    {
        // Initialize fogBarrel
        fogBarrel = FindFirstObjectByType<FogBarrel>();

        // Give everything a frame to initialize
        Invoke(nameof(InitializeEnemies), 0.01f);
    }

    private void InitializeEnemies()
    {
        // Find all enemies
        enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        if (debugMode)
        {
            Debug.Log($"[EnvironmentController] Found {(enemies != null ? enemies.Length : 0)} enemies");

            // Check what GameObjects exist in the scene
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            Debug.Log($"[EnvironmentController] Total GameObjects in scene: {allObjects.Length}");

            foreach (var obj in allObjects)
            {
                if (obj.GetComponent<EnemyAI>() != null)
                    Debug.Log($"[EnvironmentController] Found EnemyAI on: {obj.name}");
            }
        }

        // Handle case where no enemies are found
        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogWarning("[EnvironmentController] No enemies found with EnemyAI component!");
            return;
        }

        // Set all enemies to despawned initially
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetToDespawned();
                Debug.Log($"[EnvironmentController] Enemy {enemy.name} set to despawned");
            }
        }

        // Update environment state based on game progress
        UpdateEnvironment();
    }

    public void UpdateEnvironment()
    {
        if (PlayerPrefs.GetInt("AlchemyBookVolume1_Collected", 0) == 1)
        {
            if (fogBarrel != null) fogBarrel.Hide();

            if (enemies != null && enemies.Length > 0)
            {
                foreach (var enemy in enemies)
                {
                    if (enemy != null)
                    {
                        enemy.SetToRoaming();
                        Debug.Log($"[EnvironmentController] Enemy {enemy.name} set to roaming");
                    }
                }
            }
            else
            {
                Debug.LogWarning("[EnvironmentController] No enemies to set to roaming!");
            }
        }
    }
}