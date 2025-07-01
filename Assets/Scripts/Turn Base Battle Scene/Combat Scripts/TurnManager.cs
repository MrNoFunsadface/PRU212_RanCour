using System.Collections;
using UnityEngine;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Win, Lose }

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Header("Config")]
    [Tooltip("How many cards the player may play before the turn auto-ends")]
    public int maxCardsPerTurn = 3;
    private int cardsPlayedThisTurn;

    [Header("Scene References (assign in Inspector)")]
    [SerializeField] private GameObject player;
    [SerializeField] private CardSpawner cardSpawner;
    [SerializeField] private PlayerCost playerCost;
    [SerializeField] private GameOverController gameOverUI;

    private BattleState state;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log($"[TurnManager] Awake() on '{gameObject.name}' — instance assigned.");
        }
        else
        {
            Debug.Log($"[TurnManager] Duplicate on '{gameObject.name}', destroying self.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        // Wait one frame so CardSpawner.Start() has already run
        yield return null;

        // Remove any cards that were spawned by CardSpawner.Start()
        foreach (var rt in cardSpawner.cardRTList.ToArray())
            Destroy(rt.gameObject);
        cardSpawner.cardRTList.Clear();                        // :contentReference[oaicite:0]{index=0}

        // Small buffer before first turn
        yield return new WaitForSeconds(0.5f);

        BeginPlayerTurn();
    }

    private void BeginPlayerTurn()
    {
        state = BattleState.PlayerTurn;
        cardsPlayedThisTurn = 0;

        playerCost.ResetCost();
        //cardSpawner.SpawnAndFanCards();

        // TODO: enable your hand-UI interactivity here
    }

    /// <summary>
    /// Call this once per successful card play.
    /// </summary>
    public void OnCardPlayed()
    {
        cardsPlayedThisTurn++;
        if (cardsPlayedThisTurn >= maxCardsPerTurn)
            EndPlayerTurn();
    }

    public void EndPlayerTurn()
    {
        Debug.Log("[TurnManager] EndPlayerTurn() called — entering EnemyPhase");
        if (state != BattleState.PlayerTurn) return;
        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyPhase());
    }

    private IEnumerator EnemyPhase()
    {
        var enemies = FindObjectsOfType<EnemyStatus>();
        Debug.Log($"[TurnManager] EnemyPhase: found {enemies.Length} enemies");
        foreach (var e in enemies)
        {
            Debug.Log($"[TurnManager] EnemyPhase: calling PerformAction on {e.name}");
            yield return StartCoroutine(EnemyAICombat.Instance.PerformAction(e));
        }

        // Tick down any element/status durations on enemies
        foreach (var enemy in FindObjectsOfType<EnemyStatus>())
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
        if (FindObjectsOfType<EnemyStatus>().Length == 0)
        {
            state = BattleState.Win;
            gameOverUI.ShowWin();
            yield break;
        }

        // Otherwise, new player turn
        BeginPlayerTurn();
    }
}
