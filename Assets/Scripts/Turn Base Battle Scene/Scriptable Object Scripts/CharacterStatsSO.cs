using UnityEngine;

//
// Summary:
//     CharacterStatsSO is a ScriptableObject that holds the stats for a character/enemy in the game.

[CreateAssetMenu(fileName = "New CharacterStats", menuName = "Turn Base/Character/Stats", order = 3)]
public class CharacterStatsSO : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int attack;
    public int defense;
    public float attackSpeed;
    public DamageType primaryDamageType;

    [SerializeField] public GameObject charPrefab;
}
