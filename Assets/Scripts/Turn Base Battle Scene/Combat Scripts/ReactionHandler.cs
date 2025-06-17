using UnityEngine;
using System.Linq;

public class ReactionHandler : MonoBehaviour
{
    public ReactionSO[] reactions;

    public void OnCardDropped(CardSO card, EnemyStatus enemy)
    {
        enemy.ApplyElement(card);

        if (card.category != CardCategory.Element) return;

        foreach (var react in reactions)
        {
            if (react.inputElements.All(e => enemy.HasElement(e)))
            {
                CombatSystem.Instance.DealDamage(enemy.gameObject, react.damageType, react.damage, react.ignoreArmor);
                if (react.statusEffect != StatusEffect.None)
                    CombatSystem.Instance.ApplyStatus(enemy.gameObject, react.statusEffect, react.statusAmount);
                react.inputElements.ForEach(e => enemy.RemoveElement(e));
                break;
            }
        }
    }
}
