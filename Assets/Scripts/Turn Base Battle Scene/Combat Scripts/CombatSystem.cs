using System.Collections;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void DealDamage(GameObject target, DamageType type, int amount, bool ignoreArmor)
    {
        var hb = target.GetComponent<HealthBarScript>();
        if (hb != null) hb.TakeDamage(amount, ignoreArmor);
    }

    public void ApplyStatus(GameObject target, StatusEffect effect, int amount)
    {
        // implement status effect logic
    }
    public interface ITurnTaker
    {
        bool IsAlive { get; }
        IEnumerator TakeTurn();
    }
}

