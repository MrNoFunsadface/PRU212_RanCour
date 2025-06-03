using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;
}
