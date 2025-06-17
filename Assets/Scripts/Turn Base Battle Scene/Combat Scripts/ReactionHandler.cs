using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ReactionHandler : MonoBehaviour
{
    public ReactionSO[] reactions;

    public void OnCardDropped(Card card, EnemyStatus enemy)
    {
        Debug.Log("--- CARD DROPPED ---");
        Debug.Log($"Card: {card.name} | Element: {card.elementType} | Damage: {card.damage}");
        Debug.Log($"Target Enemy: {enemy.name}");

        // Always apply the element status first
        enemy.ApplyElement(card);

        if (card.category != CardCategory.Element) return;

        List<ElementalType> currentElements = enemy.GetAllElements();
        if (!currentElements.Contains(card.elementType))
        {
            currentElements.Add(card.elementType);
        }

        string before = string.Join(", ", currentElements);
        Debug.Log($"Enemy active elements before: {before}");

        foreach (var react in reactions)
        {
            if (react.inputElements.All(e => currentElements.Contains(e)))
            {
                Debug.Log($"Reaction triggered: {react.reactionName} -> Damage: {react.damage}, Status: {react.statusEffect}");
                Debug.Log($"Enemy GameObject: {enemy.gameObject.name}");
                Debug.Log($"Has CharacterStats: {enemy.GetComponent<CharacterStats>() != null}");

                CombatSystem.Instance.DealDamage(enemy.gameObject, react.damageType, react.damage, react.ignoreArmor);

                if (react.statusEffect != StatusEffect.None)
                    CombatSystem.Instance.ApplyStatus(enemy.gameObject, react.statusEffect, react.statusAmount);

                foreach (var el in react.inputElements)
                    enemy.RemoveElement(el);

                break; // Only one reaction per card
            }
        }

        string after = string.Join(", ", enemy.GetAllElements());
        Debug.Log($"Enemy active elements after: {after}");

        var stats = enemy.GetComponent<CharacterStats>();
        if (stats != null)
            Debug.Log($"Enemy health after hit: {stats.CurrentHealth}");
    }
}
