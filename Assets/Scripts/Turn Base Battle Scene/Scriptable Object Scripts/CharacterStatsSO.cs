using UnityEngine;

//
// Summary:
//     CharacterStatsSO is a ScriptableObject that holds the stats for a character/enemy in the game.
//     It also holds prefabs for enemies for spawning in free roam and battle scenes.

[CreateAssetMenu(fileName = "New CharacterStats", menuName = "Turn Base/Character/Stats", order = 3)]
public class CharacterStatsSO : ScriptableObject
{
    [Header("Basic Info")]
    public string characterName;
    public int maxHealth;
    public int attack;
    public int defense;
    public float attackSpeed;
    public DamageType primaryDamageType;
    public bool isPlayer = false;

    [Header("Character Prefabs")]
    public GameObject charPrefab;
    public GameObject freeRoamingPrefab;

    [Header("Animator")]
    public RuntimeAnimatorController animatorController;

    [Header("Animation Clip References")]
    public string idleAnimationName = "idle";
    public string attackAnimationName = "attack";
    public string hurtAnimationName = "hurt";
}
