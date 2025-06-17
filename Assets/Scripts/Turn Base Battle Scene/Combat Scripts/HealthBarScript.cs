using UnityEngine;

using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthSlider;
    public Text healthText;
    private CharacterStats characterStats;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        if (healthSlider != null)
            healthSlider.maxValue = characterStats.stats.maxHealth;
    }

    public void TakeDamage(int amount, bool ignoreArmor)
    {
        characterStats.TakeDamage(amount);
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        // Optional: implement healing
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = characterStats.stats.maxHealth - GetCurrentDamage();
        }
        if (healthText != null)
        {
            healthText.text = healthSlider.value + " / " + healthSlider.maxValue;
        }
    }

    private int GetCurrentDamage()
    {
        return characterStats.stats.maxHealth - Mathf.Max(0, characterStats.stats.maxHealth - characterStats.stats.defense);
    }
}

