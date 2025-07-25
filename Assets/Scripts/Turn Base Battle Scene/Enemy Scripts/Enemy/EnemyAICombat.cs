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

    public bool debugMode = false; // Enable debug mode for logging

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    // Animate a single enemy’s attack on the player
    public IEnumerator PerformAction(EnemyStatus enemy)
    {
        var statsMono = enemy.GetComponent<CharacterStats>();
        var dropZone = enemy.GetComponentInChildren<DropZoneScript>();
        var playerGO = GameObject.FindWithTag("Player");
        var playerAnimator = playerGO.GetComponentInChildren<Animator>();
        var playerStats = playerGO.GetComponent<CharacterStats>();

        // 1. Play Animation

        if (dropZone != null && dropZone.enemyAnimator != null)
        {
            dropZone.enemyAnimator.Play(statsMono.stats.attackAnimationName);
        }
        else
        {
            Debug.LogWarning($"[EnemyAICombat] DropZone or Animator not found for {enemy.name}.");
        }

        // 2. Small delay for animation to play
        yield return new WaitForSeconds(0.25f);

        // 3. Take attacker's stats
        if (statsMono == null || statsMono.stats == null)
        {
            Debug.LogError($"[EnemyAICombat] {enemy.name} missing CharacterStats!");
            yield break;
        }

        string enemyName = statsMono.stats.characterName;
        int dmg = statsMono.stats.attack;

        // 4. Deal damage and play sound and animation
        SoundManager.PlaySound(SoundEffectType.DAMAGETAKING);
        CombatSystem.Instance.DealDamage(
            playerGO,
            statsMono.stats.primaryDamageType,
            dmg,
            false
        );

        if (playerAnimator != null) playerAnimator.Play(statsMono.stats.hurtAnimationName);
        yield return new WaitForSeconds(0.6f);
        if (playerAnimator != null) playerAnimator.Play(statsMono.stats.idleAnimationName, 0, 0f);
        dropZone.enemyAnimator.Play(statsMono.stats.idleAnimationName, 0, 0f);

        // 5. Take hero’s remaining HP
        if (debugMode) Debug.Log($"[EnemyAICombat] playerStats: {playerStats}, player current health: {playerStats.CurrentHealth}");
        int remainingHP = playerStats != null ? playerStats.CurrentHealth : 0;
        if (debugMode) Debug.Log($"[EnemyAICombat] {enemyName} attacked Hero for {dmg} damage. Hero has {remainingHP} HP left.");

        // 6. Small cooldown before next enemy
        yield return new WaitForSeconds(0.5f);
    }
}
