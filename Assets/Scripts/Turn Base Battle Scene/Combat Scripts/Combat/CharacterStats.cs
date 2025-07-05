using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterStatsSO stats;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    private ResourceBar healthBar;

    private void Start()
    {
        healthBar = GetComponentInChildren<ResourceBar>();
        if (stats == null)
        {
            Debug.LogError($"[CharacterStats] No SO assigned on {gameObject.name}", this);
            enabled = false;
            return;
        }
        currentHealth = stats.maxHealth;
        if (healthBar != null)
        {
            Debug.Log("[CharacterStats] Health bar found, initializing");
            healthBar.Initialize(currentHealth, stats.maxHealth);
        }
    }

    public void TakeDamage(int amount)
    {
        int dmg = Mathf.Max(0, amount - stats.defense);
        currentHealth = Mathf.Max(0, currentHealth - dmg);

        Debug.Log($"[CharacterStats] {name} took {dmg} damage. Remaining HP: {currentHealth}");

        if (healthBar != null)
        {
            Debug.Log("[CharacterStats] Updating health bar");
            healthBar.UpdateResourceByAmount(-dmg);
        }
        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        int heal = Mathf.Min(amount, stats.maxHealth - currentHealth);
        currentHealth += heal;
        if (healthBar != null)
            healthBar.UpdateResourceByAmount(heal);
    }

    private void Die()
    {
        Debug.Log($"{name} died.");
        Destroy(gameObject);
    }
}
