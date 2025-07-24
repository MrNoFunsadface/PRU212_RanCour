using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum BattleState { PlayerTurn, EnemyTurn, Win, Lose }

//
// Summary:
//     TurnManager is responsible for managing the turn-based battle system.
//     It handles the player's turn, enemy actions, and game state transitions.
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Header("Scene References (assign in Inspector)")]
    [SerializeField] private GameObject player;
    [SerializeField] private CardSpawner cardSpawner;
    [SerializeField] private ResourceBar playerCost;
    [SerializeField] private GameOverController gameOverUI;

    [SerializeField] private bool debugMode = false;

    private BattleState state;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        BeginPlayerTurn();
    }

    private void BeginPlayerTurn()
    {
        if (debugMode) Debug.Log("[TurnManager] BeginPlayerTurn() called");
        state = BattleState.PlayerTurn;

        playerCost.ResetToMax();
        if(cardSpawner.SpawnAndFanCards())
        {
            if (debugMode) Debug.Log("[TurnManager] SpawnAndFanCards() triggered successfully");
        }

        // TODO: enable your hand-UI interactivity here
    }

    public void EndPlayerTurn()
    {
        if (debugMode) Debug.Log("[TurnManager] End Player Turn");
        if (state != BattleState.PlayerTurn) return;
        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyPhase());
    }

    private IEnumerator EnemyPhase()
    {
        var enemies = FindObjectsByType<EnemyStatus>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        if (debugMode) Debug.Log($"[TurnManager] EnemyPhase: found {enemies.Length} enemies");
        foreach (var e in enemies)
        {
            if (debugMode) Debug.Log($"[TurnManager] EnemyPhase: calling PerformAction on {e.name}");
            yield return StartCoroutine(EnemyAICombat.Instance.PerformAction(e));
        }

        // Tick down any element/status durations on enemies
        foreach (var enemy in FindObjectsByType<EnemyStatus>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            enemy.TickRound();

        // TODO: if you later add status effects on the player, tick them here

        // Check for defeat
        var stats = player.GetComponent<CharacterStats>();
        if (stats.CurrentHealth <= 0)
        {
            state = BattleState.Lose;
            gameOverUI.ShowLose();
            yield break;
        }

        // Check for victory
        if (FindObjectsByType<EnemyStatus>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length == 0)
        {
            state = BattleState.Win;
            gameOverUI.ShowWin();
            yield break;
        }

        // Otherwise, new player turn
        BeginPlayerTurn();
    }
}
