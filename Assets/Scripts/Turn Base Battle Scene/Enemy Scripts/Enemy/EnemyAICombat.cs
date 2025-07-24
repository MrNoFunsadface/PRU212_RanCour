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

        // 1. Play Animation
        var dropZone = enemy.GetComponentInChildren<DropZoneScript>();
        if (dropZone != null && dropZone.enemyAnimator != null)
        {
            dropZone.enemyAnimator.Play(statsMono.stats.attackAnimationName);
            SoundManager.PlaySound(dropZone.attackSoundType);
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

        // 4. Deal damage
        SoundManager.PlaySound(SoundEffectType.DAMAGETAKING);
        var playerGO = GameObject.FindWithTag("Player");
        CombatSystem.Instance.DealDamage(
            playerGO,
            statsMono.stats.primaryDamageType,
            dmg,
            false
        );

        if (playerGO.TryGetComponent<Animator>(out var playerAnimator))
        {
            playerAnimator.Play(statsMono.stats.hurtAnimationName);
            yield return new WaitForSeconds(0.6f);
            playerAnimator.Play(statsMono.stats.idleAnimationName, 0, 0f);
            dropZone.enemyAnimator.Play(statsMono.stats.idleAnimationName, 0, 0f);
        }
        else
        {
            Debug.LogWarning($"[EnemyAICombat] Player Animator not found for {enemyName} attack animation.");
        }

        // 5. Take hero’s remaining HP
        var playerStats = playerGO.GetComponent<CharacterStats>();
        if (debugMode) Debug.Log($"[EnemyAICombat] playerStats: {playerStats}, player current health: {playerStats.CurrentHealth}");
        int remainingHP = playerStats != null ? playerStats.CurrentHealth : 0;
        if (debugMode) Debug.Log($"[EnemyAICombat] {enemyName} attacked Hero for {dmg} damage. Hero has {remainingHP} HP left.");


        // 6. Small cooldown before next enemy
        yield return new WaitForSeconds(0.5f);
    }
}
