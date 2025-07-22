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

    [Header("Enemy wave configuration")]
    [SerializeField] private List<EnemyWithStats> enemyTable;
    [SerializeField] private int minEnemyCount = 1; // Minimum number of enemies to spawn
    [SerializeField] private int maxEnemyCount = 4; // Maximum number of enemies to spawn
    [SerializeField] private int dungeonBaseLevel = 1; // Base level for the dungeon (maybe used for later)
    [SerializeField] private int waveValue = 10; // Total value of the wave

    private Vector2 enemyReturnPosition;

    private void Start()
    {
        if (GameManager.Instance.isEnemyReturningFromBattle &&
            GameManager.Instance.lastBattleSpawnerId == spawnerId)
        {
            // Get position from GameManager
            enemyReturnPosition = GameManager.Instance.enemyReturnPosition;
            Debug.Log($"[RoamingEnemySpawner] Spawner {spawnerId} returning from battle, position: {enemyReturnPosition}");
        }
        MobWaveDataManager.GetWaveBySpawner(spawnerId, out MobWaveData existingWaveData);
        if (existingWaveData != null) InstantiateRepEnemy(existingWaveData);
        else SpawnEnemyWave();
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
            repEnemyObject = Instantiate(waveData.enemies[0].enemy.freeRoamingPrefab, enemyReturnPosition, Quaternion.identity);
            // Reset the flag
            GameManager.Instance.isEnemyReturningFromBattle = false;
        }
        else
        {
            repEnemyObject = Instantiate(waveData.enemies[0].enemy.freeRoamingPrefab, transform.position, Quaternion.identity);
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