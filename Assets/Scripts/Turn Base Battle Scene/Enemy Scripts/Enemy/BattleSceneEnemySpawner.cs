using System.Collections.Generic;
using UnityEngine;
//
// Summary:
//     EnemySpawnerScript is responsible for spawning enemies in the battle scene.
//     It manages the enemy and its related components.

public class BattleSceneEnemySpawner : MonoBehaviour
{
    public bool characterStatsDebugMode = false; // Debug mode for character stats
    private CharacterStats[] characterStats; // Array to hold character stats

    [Header("Spawner and Objects attached with the enemy")]
    [SerializeField] private Transform spawner;
    [SerializeField] private GameObject dropZonePrefab;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject activeHealthBar; // Health bar when hover the card over the enemy's drop zone

    [Header("Enemy spawning configure")]
    [SerializeField] private EnemyWithStats[] enemyTable;
    [SerializeField] private List<EnemyWithStats> fallBackEnemyToSpawnList; // Fallback enemies to spawn if wave data is not found
    [SerializeField] private CharacterStatsSO representativeEnemy; // The representative enemy (first one), take from free roam scene
    [SerializeField] private int minEnemyCount; // Minimum number of enemies to spawn
    [SerializeField] private int maxEnemyCount; // Maximum number of enemies to spawn

    [SerializeField] private int dungeonBaseLevel;
    [SerializeField] private int waveValue;

    private readonly List<EnemyWithStats> enemyToSpawn = new();

    private void Start()
    {
        SpawnEnemy();
        characterStats = GetComponentsInChildren<CharacterStats>();
        foreach(var stat in characterStats)
        {
            stat.debugMode = characterStatsDebugMode; // Set debug mode for each character stats
        }
    }

    public void SpawnEnemy()
    {
        string waveId = BattleTransitionData.SelectedWaveId;
        MobWaveDataManager.GetWave(waveId, out MobWaveData waveData);
        if (waveData == null)
        {
            Debug.LogWarning($"[EnemySpawnerScript] Wave data for ID {waveId} not found. Cannot spawn enemies.");
            // Fallback to a predefined list of enemies if wave data is not found
            if (fallBackEnemyToSpawnList.Count == 0)
            {
                Debug.LogError("[EnemySpawnerScript] No fallback enemies defined. Cannot proceed with spawning.");
                return;
            }
            else
            {
                Debug.LogWarning($"[EnemySpawnerScript] Using fallback enemies for wave ID {waveId}.");
                enemyToSpawn.AddRange(fallBackEnemyToSpawnList);
            }
        }
        else
        {
            Debug.Log($"[EnemySpawnerScript] Wave data for ID {waveId} found. Proceeding to spawn enemies.");
            enemyToSpawn.AddRange(waveData.enemies);
        }

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
    }
}


