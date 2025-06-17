using UnityEngine;

//
// Summary:
//     Enemy is a ScriptableObject that represents an enemy in the game.

[CreateAssetMenu(fileName = "New Enemy", menuName = "Turn Base/Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;
}
