// EnemyCombatAI.cs
using System.Collections;
using System.Linq;
using UnityEngine;
using static CombatSystem;

public class EnemyCombatAI : MonoBehaviour, ITurnTaker
{
    [Header("Stats & Logic")]
    public CharacterStats stats;
    public ReactionHandler reactionHandler;
    public Card[] enemyDeck;        // assign possible cards in Inspector
    public float actionDelay = 1f;  // pause between actions

    public bool IsAlive => stats.CurrentHealth > 0;

    public IEnumerator TakeTurn()
    {
        // pick a random Element or Compound card
        var card = enemyDeck
            .Where(c => c.category != CardCategory.Catalyst)
            .OrderBy(_ => Random.value)
            .FirstOrDefault();

        if (card == null)
        {
            Debug.LogWarning($"{name} has no usable cards!");
            yield break;
        }

        Debug.Log($"{name} plays {card.cardName}");

        // target the player’s status (assumes PlayerControllerCombat has EnemyStatus)
        var playerStatus = TurnManager.Instance.player
            .GetComponent<EnemyStatus>();
        if (playerStatus == null)
        {
            Debug.LogError("Player is missing EnemyStatus component!");
            yield break;
        }

        reactionHandler.OnCardDropped(card, playerStatus);

        yield return new WaitForSeconds(actionDelay);
    }
}
