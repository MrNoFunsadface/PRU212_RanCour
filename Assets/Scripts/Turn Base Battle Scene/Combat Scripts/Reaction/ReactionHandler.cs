using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ReactionHandler : MonoBehaviour
{
    public static ReactionHandler Instance { get; private set; }

    [SerializeField] private bool debugMode = false;

    public ReactionSO[] reactions;
    private void Awake()
    {
        // Singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void OnCardDropped(CardSO card, EnemyStatus enemy)
    {
        if (debugMode) Debug.Log($"[ReactionHandler] Card dropped: {card.cardName} ({card.elementType}) on {enemy.name}");

        // 1) apply the status/duration/count
        enemy.ApplyElement(card);

        // 2) skip only Catalysts
        if (card.category == CardCategory.Catalyst) return;

        // 3) build the multiset of chemicals we HAVE
        var have = new List<ElementalType>();
        foreach (var type in enemy.GetAllElements())
            have.AddRange(Enumerable.Repeat(type, enemy.GetElementCount(type)));

        // ensure the just?dropped card is included
        have.Add(card.elementType);

        if (debugMode) Debug.Log($"[ReactionHandler] Chemicals now: {string.Join(", ", have)}");

        // 4) check each reaction for an exact multiset match
        foreach (var react in reactions)
        {
            if (Matches(have, react.inputElements))
            {
                if (debugMode) Debug.Log($"[ReactionHandler] Reaction {react.reactionName}: {string.Join(" + ", react.inputElements)} ? damage {react.damage}");

                // apply combat effects
                CombatSystem.Instance.DealDamage(enemy.gameObject,
                                                 react.damageType,
                                                 react.damage,
                                                 react.ignoreArmor);
                if (react.statusEffect != StatusEffect.None)
                    CombatSystem.Instance.ApplyStatus(enemy.gameObject,
                                                      react.statusEffect,
                                                      react.statusAmount);

                // consume each reactant exactly once
                foreach (var need in react.inputElements)
                    enemy.RemoveElement(need);

                break; // only one reaction per drop
            }
        }
    }

    // returns true if 'have' contains each element in 'need' at least as many times as listed
    private bool Matches(List<ElementalType> have, List<ElementalType> need)
    {
        var haveCounts = have.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        var needCounts = need.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

        foreach (var kv in needCounts)
        {
            if (!haveCounts.ContainsKey(kv.Key) || haveCounts[kv.Key] < kv.Value)
                return false;
        }
        return true;
    }
}
