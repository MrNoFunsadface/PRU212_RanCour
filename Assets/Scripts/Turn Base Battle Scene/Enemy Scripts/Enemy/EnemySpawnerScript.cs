using System.Collections.Generic;
using UnityEngine;
//
// Summary:
//     EnemySpawnerScript is responsible for spawning enemies in the battle scene.
//     It manages the enemy and its related components.

public class EnemySpawnerScript : MonoBehaviour
{
    [Header("Spawner and Objects attached with the enemy")]
    [SerializeField] private Transform spawner;
    [SerializeField] private GameObject dropZonePrefab;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject activeHealthBar; // Health bar when hover the card over the enemy's drop zone

    [Header("Enemy spawning configure")]
    [SerializeField] private EnemyWithStats[] enemyTable;
    [SerializeField] private List<EnemyWithStats> enemyToSpawn;
    [SerializeField] private CharacterStatsSO representativeEnemy; // The representative enemy (first one), take from free roam scene
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
        // REMOVE COMMENT TO USE THE RANDOMIZED ENEMY SPAWNING LOGIC

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
            CharacterStatsSO enemy = enemyToSpawn[i].enemy;
            CreateEnemyObject(enemy, dropZonePrefab, healthBar, activeHealthBar, spawner, i);
        }
    }

    public void CreateEnemyObject(CharacterStatsSO enemy, GameObject dropZonePrefab, GameObject healthBar, GameObject activeHealthBar, Transform spawner, int order)
    {
        GameObject enemyObject = Instantiate(enemy.charPrefab, spawner);
        GameObject dropZoneObject = Instantiate(dropZonePrefab, enemyObject.transform);
        GameObject activeHealthBarObject = Instantiate(activeHealthBar, enemyObject.transform);
        GameObject healthBarObject = Instantiate(healthBar, enemyObject.transform);


        var dropZoneScript = dropZoneObject.GetComponent<DropZoneScript>();
        dropZoneScript.enemyName = enemy.characterName;
        dropZoneScript.enemyOrder = order;

        dropZoneScript.activeHealthBar = activeHealthBarObject;
        dropZoneScript.healthBar = healthBarObject;

        Debug.Log($"Drop zone for {dropZoneScript.enemyName} created with order {dropZoneScript.enemyOrder}");
    }
}

[System.Serializable]
public class EnemyWithStats
{
    public CharacterStatsSO enemy;
    public int cost;
    public int baseHealth;

    // Parameterless constructor for Unity serialization
    public EnemyWithStats() { }

    public EnemyWithStats(CharacterStatsSO enemy, int cost)
    {
        this.enemy = enemy;
        this.cost = cost;
    }
}
