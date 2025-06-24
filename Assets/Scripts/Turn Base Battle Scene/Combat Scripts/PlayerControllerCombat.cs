using UnityEngine;
using System.Collections;

public class PlayerControllerCombat : MonoBehaviour, CombatSystem.ITurnTaker
{
    public CharacterStats stats;

    public bool IsAlive => stats.CurrentHealth > 0;

    public IEnumerator TakeTurn()
    {
        Debug.Log("PlayerCombat Turn: play a card");
        bool played = false;
        System.Action onPlay = () => played = true;
        CardDrag.OnCardPlayed += onPlay;

        // wait until DiscardCard() invokes OnCardPlayed
        while (!played) yield return null;

        CardDrag.OnCardPlayed -= onPlay;

        // brief pause before next turn
        yield return new WaitForSeconds(0.2f);
    }
}
