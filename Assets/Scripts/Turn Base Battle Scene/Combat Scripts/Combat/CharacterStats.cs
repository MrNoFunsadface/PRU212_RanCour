using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterStatsSO stats;

    public bool debugMode = false; // Enable this to see debug logs in the console

    private int currentHealth;
    public int CurrentHealth => currentHealth;

    private ResourceBar[] resourceBars;
    private ResourceBar healthBar; // The health bar shown in the battle scene
    private ResourceBar activeHealthBar; // The health bar shown when hovering over the enemy's drop zone

    private void Start()
    {
        InitializeHealthBars();
    }

    private void InitializeHealthBars()
    {
        resourceBars = GetComponentsInChildren<ResourceBar>();
        if (debugMode) Debug.Log($"[CharacterStats] Found {resourceBars.Length} resource bars in {gameObject.name}");
        switch (resourceBars.Length)
        {
            case 0:
                if (debugMode) Debug.LogWarning($"[CharacterStats] No resource bars found on {gameObject.name}");
                break;
            case 1:
                if (debugMode) Debug.Log($"[CharacterStats] Only one resource bar found on {gameObject.name}, using it for both health and active health.");
                healthBar = activeHealthBar = resourceBars[0];
                break;
            case 2:
                if (debugMode) Debug.Log("[CharacterStats] Two resource bars found, using them for health and active health.");
                healthBar = resourceBars[1];
                activeHealthBar = resourceBars[0];
                break;
            default:
                if (debugMode) Debug.LogError($"[CharacterStats] More than two resource bars found on {gameObject.name}, this is not supposed to happen!");
                return;
        }

        if (stats == null)
        {
            if (debugMode) Debug.LogError($"[CharacterStats] No SO assigned on {gameObject.name}", this);
            enabled = false;
            return;
        }

        currentHealth = stats.maxHealth;
        if (debugMode) Debug.Log($"[CharacterStats] {name} initialized with {currentHealth}/{stats.maxHealth} HP");
        if (healthBar != null)
        {
            if (debugMode) Debug.Log("[CharacterStats] Health bar found, initializing");
            healthBar.Initialize(currentHealth, stats.maxHealth);
        }
        if (activeHealthBar != null)
        {
            if (debugMode) Debug.Log("[CharacterStats] Active health bar found, initializing");
            activeHealthBar.Initialize(currentHealth, stats.maxHealth);
        }
    }

    public void TakeDamage(int amount)
    {
        int dmg = Mathf.Max(0, amount - stats.defense);
        currentHealth = Mathf.Max(0, currentHealth - dmg);

        if (debugMode) Debug.Log($"[CharacterStats] {name} took {dmg} damage. Remaining HP: {currentHealth}");

        if (stats.isPlayer)
        {
            BattleLog.Instance.LogBattleEvent(
                $"Took {dmg} damage. Remaining HP: {currentHealth}"
            );
            BattleLog.Instance.UpdateDisplayer();
        }
        else
        {
            BattleLog.Instance.LogBattleEvent(
                $"Attacked {stats.characterName}, dealt {dmg} damage. Remaining HP: {currentHealth}"
            );
            BattleLog.Instance.UpdateDisplayer();
        }

        if (healthBar != null)
        {
            if (debugMode) Debug.Log("[CharacterStats] Updating health bar");
            healthBar.UpdateResourceByAmount(-dmg);
        }
        if (activeHealthBar != null)
        {
            if (debugMode) Debug.Log("[CharacterStats] Updating active health bar");
            activeHealthBar.UpdateResourceByAmount(-dmg);
        }
        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        int heal = Mathf.Min(amount, stats.maxHealth - currentHealth);
        currentHealth += heal;
        if (healthBar != null) healthBar.UpdateResourceByAmount(heal);
        if (activeHealthBar != null) activeHealthBar.UpdateResourceByAmount(heal);
    }

    private void Die()
    {
        if (debugMode) Debug.Log($"[CharacterStats] {name} died.");
        Destroy(gameObject);
    }
}
