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
    }
    private void Start()
    {
        healthSlider.maxValue = characterStats.stats.maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount, bool ignoreArmor)
    {
        characterStats.TakeDamage(amount);
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        // Optional: implement healing
        characterStats.Heal(amount);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = characterStats.CurrentHealth;

        if (healthText != null)
            healthText.text = $"{characterStats.CurrentHealth} / {characterStats.stats.maxHealth}";
    }
}
