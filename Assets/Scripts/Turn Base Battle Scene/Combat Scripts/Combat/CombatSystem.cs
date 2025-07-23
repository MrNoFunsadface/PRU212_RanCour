using UnityEngine;

//
// Summary:
//     CombatSystem is a singleton that manages combat interactions in the game. It handles damage dealing,
//     status effects, and other combat-related logic. It is designed to be used by both player and enemy scripts

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance;
    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void DealDamage(GameObject target, DamageType type, int amount, bool ignoreArmor)
    {
        if (target.TryGetComponent<CharacterStats>(out var action))
        {
            Debug.Log("[CombatSystem] CharacterStats get successfully from target");
            action.TakeDamage(amount);
        }
        else Debug.Log("[CombatSystem] CharacterStats failed to get");
    }

    public void ApplyStatus(GameObject target, StatusEffect effect, int amount)
    {
        // implement status effect logic
    }
}

