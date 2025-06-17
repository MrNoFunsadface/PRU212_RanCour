using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterStatsSO stats;
    private int currentHealth;

    public int CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = stats.maxHealth;
    }

    public void TakeDamage(int amount)
    {
        int dmg = Mathf.Max(0, amount - stats.defense);
        currentHealth = Mathf.Max(0, currentHealth - dmg);

        Debug.Log($"{name} took {dmg} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, stats.maxHealth);
    }

    private void Die()
    {
        // Optional: trigger death animation or disable
        Debug.Log($"{name} died.");
        Destroy(gameObject);
    }
}
