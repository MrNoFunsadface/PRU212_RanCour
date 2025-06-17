using UnityEngine;
using System.Collections.Generic;

public class EnemyStatus : MonoBehaviour
{
    private Dictionary<ElementalType, int> elementTimers = new Dictionary<ElementalType, int>();
    private HashSet<ElementalType> compoundsAppliedOnce = new HashSet<ElementalType>();

    public void ApplyElement(CardSO card)
    {
        var type = card.elementType;
        if (type == ElementalType.None) return;

        if (card.category == CardCategory.Compound && card.damageOnApplyOnly && !compoundsAppliedOnce.Contains(type))
        {
            CombatSystem.Instance.DealDamage(gameObject, card.damageType, card.damage, card.ignoreArmor);
            compoundsAppliedOnce.Add(type);
        }

        if (elementTimers.ContainsKey(type))
        {
            if (card.isStackable) elementTimers[type] += card.elementDuration;
        }
        else
        {
            elementTimers[type] = card.elementDuration;
        }
    }

    public bool HasElement(ElementalType type)
    {
        return elementTimers.ContainsKey(type);
    }

    public void RemoveElement(ElementalType type)
    {
        elementTimers.Remove(type);
        compoundsAppliedOnce.Remove(type);
    }

    public void TickRound()
    {
        var keys = new List<ElementalType>(elementTimers.Keys);
        foreach (var key in keys)
        {
            elementTimers[key]--;
            if (elementTimers[key] <= 0) RemoveElement(key);
        }
    }
}