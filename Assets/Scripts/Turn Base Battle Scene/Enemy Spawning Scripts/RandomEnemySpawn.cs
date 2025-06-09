using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawn : MonoBehaviour
{
    [Header("Spawner and Objects attached with the enemy")]
    [SerializeField] private Transform spawner;
    [SerializeField] private GameObject dropZonePrefab;
    [SerializeField] private GameObject healthBar;

    [Header("Enemy spawning configure")]
    [SerializeField] private EnemyWithStats[] enemyTable;
    [SerializeField] private List<EnemyWithStats> enemyToSpawn;
    [SerializeField] private Enemy representativeEnemy; // The representative enemy (first one), take from free roam scene
    [SerializeField] private int minEnemyCount; // Minimum number of enemies to spawn
    [SerializeField] private int maxEnemyCount; // Maximum number of enemies to spawn

    [SerializeField] private int dungeonBaseLevel;
    [SerializeField] private int waveValue;

    private void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        /*
        // Defensive copy of waveValue to avoid modifying the serialized field
        int remainingWaveValue = waveValue;

        // Find representative enemy cost
        int repCost = 0;
        for (int i = 0; i < enemyTable.Length; i++)
        {
            if (enemyTable[i].enemy == representativeEnemy)
            {
                repCost = enemyTable[i].cost;
                break;
            }
        }

        // Always spawn the representative enemy first
        var generatedEnemies = new List<EnemyWithStats>
        {
            new(representativeEnemy, repCost)
        };
        remainingWaveValue -= repCost;

        // Determine how many enemies to spawn (including representative)
        int enemyCount = Random.Range(minEnemyCount, maxEnemyCount + 1); // +1 to make max inclusive

        // Randomly add enemies (any type, including representative) until we reach enemyCount or run out of points
        while (generatedEnemies.Count < enemyCount && remainingWaveValue > 0)
        {
            // Filter affordable enemies
            var affordable = new List<EnemyWithStats>();
            foreach (var entry in enemyTable)
            {
                if (entry.cost <= remainingWaveValue)
                    affordable.Add(entry);
            }
            if (affordable.Count == 0)
                break;

            int randomIndex = Random.Range(0, affordable.Count);
            var chosen = affordable[randomIndex];
            generatedEnemies.Add(chosen);
            remainingWaveValue -= chosen.cost;
        }

        // Update enemyToSpawn list
        enemyToSpawn.Clear();
        enemyToSpawn.AddRange(generatedEnemies);
        */

        // Spawn enemies and drop zones
        for (int i = 0; i < enemyToSpawn.Count; i++)
        {
            Enemy enemy = enemyToSpawn[i].enemy;
            CreateEnemyObject(enemy, dropZonePrefab, healthBar, spawner, i);
        }
    }

    public void CreateEnemyObject(Enemy enemy, GameObject dropZonePrefab, GameObject healthBar, Transform spawner, int order)
    {
        GameObject enemyObject = Instantiate(enemy.enemyPrefab, spawner);
        GameObject dropZoneObject = Instantiate(dropZonePrefab, enemyObject.transform);
        GameObject healthBarObject = Instantiate(healthBar, enemyObject.transform);

        var dropZoneScript = dropZoneObject.GetComponent<DropZoneScript>();
        dropZoneScript.enemyName = enemy.enemyName;
        dropZoneScript.enemyOrder = order;
        Debug.Log($"Drop zone for {dropZoneScript.enemyName} created with order {dropZoneScript.enemyOrder}");
    }
}

[System.Serializable]
public class EnemyWithStats
{
    public Enemy enemy;
    public int cost;
    public int baseHealth;

    // Parameterless constructor for Unity serialization
    public EnemyWithStats() { }

    public EnemyWithStats(Enemy enemy, int cost)
    {
        this.enemy = enemy;
        this.cost = cost;
    }
}
