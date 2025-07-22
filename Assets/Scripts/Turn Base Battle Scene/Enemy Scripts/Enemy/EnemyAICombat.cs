// EnemyAI.cs
using System.Collections;
using UnityEngine;

//
// Summary:
//     EnemyAICombat handles the combat actions of enemies. It manages the attack animations,
//     damage calculations taken from CombatSystem, and logging of battle events.

public class EnemyAICombat : MonoBehaviour
{
    public static EnemyAICombat Instance { get; private set; }

    private void Awake()
    {
        // Singleton
        if (Instance == null) 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Animate a single enemy’s attack on the player
    public IEnumerator PerformAction(EnemyStatus enemy)
    {
        // 1) small delay for clarity
        yield return new WaitForSeconds(0.5f);

        // 2) grab its attack stats
        var statsMono = enemy.GetComponent<CharacterStats>();
        if (statsMono == null || statsMono.stats == null)
        {
            Debug.LogError($"[EnemyAICombat] {enemy.name} missing CharacterStats!");
            yield break;
        }

        string enemyName = statsMono.stats.characterName;       // e.g. “Bat”
        int dmg = statsMono.stats.attack;

        // 3) deal the damage
        var playerGO = GameObject.FindWithTag("Player");
        CombatSystem.Instance.DealDamage(
            playerGO,
            statsMono.stats.primaryDamageType,
            dmg,
            false
        );

        // 4) LOG IT to your BattleLog UI
        var battleLog = BattleLog.Instance;
        if (battleLog != null)
        {
            // grab hero’s remaining HP
            var playerStats = playerGO.GetComponent<CharacterStats>();
            int remainingHP = playerStats != null
                ? playerStats.CurrentHealth
                : 0;

            battleLog.LogBattleEvent(
                $"{enemyName} attacked Hero for {dmg} damage. Hero has {remainingHP} HP left."
            );
            battleLog.UpdateDisplayer();
            Debug.Log($"{enemyName} attacked Hero for {dmg} damage. Hero has {remainingHP} HP left.");
        }
        else
        {
            Debug.LogWarning("[EnemyAICombat] BattleLogScript.Instance is null—cannot write attack log.");
        }

        // 5) small cooldown before next enemy
        yield return new WaitForSeconds(0.5f);
    }
}
