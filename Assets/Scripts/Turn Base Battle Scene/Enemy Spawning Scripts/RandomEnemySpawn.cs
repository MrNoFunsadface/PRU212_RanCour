using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawn : MonoBehaviour
{
    [SerializeField] private Enemy[] enemyToSpawn;
    [SerializeField] private Transform spawner;
    [SerializeField] private GameObject dropZone;

    // Later changed to automatically scaled
    [SerializeField] private int dungeonBaseLevel;
    [SerializeField] private int point;

    private List<Enemy> enemyList = new List<Enemy>();

    private void Awake()
    {
    }
    private void Start()
    {
        SpawnEnemy();
    }

    public int WavePointCalculation(int dungeonBaseLevel)
    {
        point = 0;
        return 0;
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < enemyToSpawn.Length; i++)
        {
            Enemy enemy = enemyToSpawn[i];
            enemyList.Add(enemy);

            AppendDropZoneWithEnemy(enemy, dropZone, spawner);
        }
    }

    public void AppendDropZoneWithEnemy(Enemy enemy, GameObject dropZone, Transform spawner)
    {
        // Instantiate the enemy prefab at the spawner's position
        GameObject enemyObject = Instantiate(enemy.enemyPrefab, spawner);

        // Instantiate the drop zone prefab at the enemy's position
        GameObject dropZoneObject = Instantiate(dropZone, enemyObject.transform);
    }
}
