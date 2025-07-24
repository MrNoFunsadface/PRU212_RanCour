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
        // sanity checks
        if (characterStats == null || characterStats.stats == null)
        {
            Debug.LogError($"[HealthBar] Missing CharacterStats or SO on {name}");
            enabled = false;
            return;
        }
        if (healthSlider == null || healthText == null)
        {
            Debug.LogError($"[HealthBar] Slider or Text not hooked up on {name}");
            enabled = false;
            return;
        }

        // now safe to initialize
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
