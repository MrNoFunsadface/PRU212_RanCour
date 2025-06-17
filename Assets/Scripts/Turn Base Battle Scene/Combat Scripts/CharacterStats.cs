using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterStatsSO stats;
    private int currentHealth;

    private void Start()
    {
        currentHealth = stats.maxHealth;
    }

    public void TakeDamage(int amount)
    {
        int dmg = Mathf.Max(0, amount - stats.defense);
        currentHealth -= dmg;
        if (currentHealth <= 0) Die();
    }

    public void DealDamage(GameObject target)
    {
        CombatSystem.Instance.DealDamage(target, stats.primaryDamageType, stats.attack, false);
    }

    private void Die()
    {
        // Handle death (e.g., animation, destroy)
        Destroy(gameObject);
    }
}

