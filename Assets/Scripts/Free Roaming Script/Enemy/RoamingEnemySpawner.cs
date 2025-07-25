using System.Collections.Generic;
using UnityEngine;

//
// Summary:
//     RoamingEnemySpawner is responsible for generating a wave of roaming enemies, saving
//     it to MobWaveDataManager, and instantiating a representative enemy in the scene.
public class RoamingEnemySpawner : MonoBehaviour
{
    [Header("Spawner info")]
    [SerializeField] private string spawnerId; // Unique identifier for the spawner
    [SerializeField] private string fromSceneName; // Name of the scene this spawner is from
    private bool isVictory; // Indicate spawner has been won

    [Header("Enemy wave configuration")]
    [SerializeField] private List<EnemyWithStats> enemyTable;
    [SerializeField] private int minEnemyCount = 1; // Minimum number of enemies to spawn
    [SerializeField] private int maxEnemyCount = 4; // Maximum number of enemies to spawn
    [SerializeField] private int dungeonBaseLevel = 1; // Base level for the dungeon (maybe used for later)
    [SerializeField] private int waveValue = 10; // Total value of the wave

    private Vector2 enemyReturnPosition;

    private void Start()
    {
        InitializeRoamingEnemySpawner();
    }

    private void InitializeRoamingEnemySpawner()
    {
        // Check for victory directly from PlayerPrefs first (for cross-session persistence)
        MobWaveDataManager.GetWaveBySpawner(spawnerId, out MobWaveData existingWaveData);
        if (existingWaveData != null && existingWaveData.isVictory)
        {
            isVictory = true;
            Debug.Log($"[RoamingEnemySpawner] Spawner {spawnerId} has a victorious wave and won't spawn enemies.");
            return;
        }
        GameManager.Instance.fromSceneName = fromSceneName; // Store the scene name for returning

        Debug.Log($"GameManager.Instance: {GameManager.Instance}");
        Debug.Log($"GameManager.Instance.isEnemyReturningFromBattle: {GameManager.Instance.isEnemyReturningFromBattle}");
        Debug.Log($"GameManager.Instance.lastBattleSpawnerId == spawnerId: {GameManager.Instance.lastBattleSpawnerId == spawnerId}");
        // Check if returning from battle and this was the spawner
        if (GameManager.Instance != null &&
            GameManager.Instance.isEnemyReturningFromBattle &&
            GameManager.Instance.lastBattleSpawnerId == spawnerId)
        {
            
            Debug.Log($"[RoamingEnemySpawner] Spawner {spawnerId} is returning from battle scene: {GameManager.Instance.fromSceneName}");

            // Check if the player won the battle
            if (GameManager.Instance.lastBattleResult)
            {
                isVictory = true;
                // No need to spawn anything if the player won
                Debug.Log($"[RoamingEnemySpawner] Spawner {spawnerId} marked as victorious!");
                return;
            }
            else
            {
                // Player lost, set enemy return position
                enemyReturnPosition = GameManager.Instance.enemyReturnPosition;
                Debug.Log($"[RoamingEnemySpawner] Spawner {spawnerId} returning from lost battle, position: {enemyReturnPosition}");
            }
        }

        // If the spawner is already marked as victorious, don't spawn anything
        if (isVictory)
        {
            Debug.Log($"[RoamingEnemySpawner] Spawner {spawnerId} is already marked as victorious. No enemies will spawn.");
            return;
        }

        // Check for existing wave data
        if (existingWaveData != null)
            InstantiateRepEnemy(existingWaveData);
        else
            SpawnEnemyWave();
    }

    public void SpawnEnemyWave()
    {
        // Generate random wave of enemy based on min, max count and wave value
        int remainingValue = waveValue;
        var generatedEnemies = new List<EnemyWithStats>();
        if (minEnemyCount < 1) minEnemyCount = 1; // Ensure minimum count is at least 1
        if (maxEnemyCount < minEnemyCount) maxEnemyCount = minEnemyCount; // Ensure max count is at least min count

        int enemyCount = Random.Range(minEnemyCount, maxEnemyCount + 1);

        while (generatedEnemies.Count < enemyCount && remainingValue > 0)
        {
            var affordable = new List<EnemyWithStats>();
            foreach (var entry in enemyTable)
                if (entry.cost <= remainingValue)
                    affordable.Add(entry);

            if (affordable.Count == 0) break; // No affordable enemies left

            var randomIndex = Random.Range(0, affordable.Count);
            var chosen = affordable[randomIndex];
            affordable.RemoveAt(randomIndex);
            generatedEnemies.Add(chosen); // Add chosen enemy to the chosen list
            remainingValue -= chosen.cost; // Deduct cost from remaining value
        }

        // Save wave data
        var waveId = System.Guid.NewGuid().ToString();
        var waveData = new MobWaveData(waveId, spawnerId, generatedEnemies);

        InstantiateRepEnemy(waveData); // Instantiate representative enemy
        MobWaveDataManager.AddWave(waveData);
    }

    private void InstantiateRepEnemy(MobWaveData waveData)
    {
        GameObject repEnemyObject;

        // Use the stored position if this is the spawner from the last battle
        if (GameManager.Instance.isEnemyReturningFromBattle &&
            GameManager.Instance.lastBattleSpawnerId == spawnerId)
        {
            repEnemyObject = Instantiate(waveData.enemies[0].enemy.freeRoamingPrefab, enemyReturnPosition, Quaternion.identity, transform);
            // Reset the flag
            GameManager.Instance.isEnemyReturningFromBattle = false;
        }
        else
        {
            repEnemyObject = Instantiate(waveData.enemies[0].enemy.freeRoamingPrefab, transform);
        }

        // Attach controller and set properties
        var controller = repEnemyObject.AddComponent<RoamingEnemyController>();
        controller.waveId = waveData.waveId;
        controller.spawnerId = spawnerId;
    }
}

[System.Serializable]
public class EnemyWithStats
{
    public CharacterStatsSO enemy;
    public int cost;

    // Parameterless constructor for Unity serialization
    public EnemyWithStats() { }

    public EnemyWithStats(CharacterStatsSO enemy, int cost)
    {
        this.enemy = enemy;
        this.cost = cost;
    }
}