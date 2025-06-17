using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterStats", menuName = "Characters/Stats", order = 3)]
public class CharacterStatsSO : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int attack;
    public int defense;
    public float attackSpeed;
    public DamageType primaryDamageType;
}