using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyStatus : MonoBehaviour
{
    // tracks remaining duration per chemical (for expiry)
    private Dictionary<ElementalType, int> elementTimers = new Dictionary<ElementalType, int>();
    // tracks how many cards of each type have been applied
    private Dictionary<ElementalType, int> elementCounts = new Dictionary<ElementalType, int>();

    private HashSet<ElementalType> compoundsAppliedOnce = new HashSet<ElementalType>();

    public void ApplyElement(Card card)
    {
        var type = card.elementType;
        if (type == ElementalType.None) return;

        // handle one-time compound damage
        if (card.category == CardCategory.Compound &&
            card.damageOnApplyOnly &&
            !compoundsAppliedOnce.Contains(type))
        {
            CombatSystem.Instance.DealDamage(gameObject, card.damageType, card.damage, card.ignoreArmor);
            compoundsAppliedOnce.Add(type);
        }

        // duration logic (for expiry)
        if (elementTimers.ContainsKey(type))
        {
            if (card.isStackable)
                elementTimers[type] += card.elementDuration;
        }
        else elementTimers[type] = card.elementDuration;

        // count logic (for stoichiometry)
        if (elementCounts.ContainsKey(type))
            elementCounts[type]++;
        else
            elementCounts[type] = 1;
    }

    // how many of this chemical are currently applied
    public int GetElementCount(ElementalType type)
    {
        return elementCounts.TryGetValue(type, out var c) ? c : 0;
    }

    // returns each distinct chemical currently present
    public List<ElementalType> GetAllElements()
        => elementCounts.Keys.ToList();

    // remove ONE stack of this chemical (used by ReactionHandler)
    public void RemoveElement(ElementalType type)
    {
        if (elementCounts.TryGetValue(type, out var c))
        {
            if (c > 1) elementCounts[type] = c - 1;
            else elementCounts.Remove(type);
        }

        // if no more stacks, remove timer & one-time flags
        if (!elementCounts.ContainsKey(type))
        {
            elementTimers.Remove(type);
            compoundsAppliedOnce.Remove(type);
        }
    }

    // tick down durations & remove expired
    public void TickRound()
    {
        var expired = new List<ElementalType>();
        foreach (var kv in elementTimers)
        {
            elementTimers[kv.Key] = kv.Value - 1;
            if (elementTimers[kv.Key] <= 0)
                expired.Add(kv.Key);
        }
        foreach (var e in expired)
            RemoveElement(e);
    }

    public bool HasElement(ElementalType type)
        => elementCounts.ContainsKey(type);
}
