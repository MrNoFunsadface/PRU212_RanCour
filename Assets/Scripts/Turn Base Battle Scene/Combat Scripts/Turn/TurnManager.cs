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
    [SerializeField] private GameOverController gameOverController;

    [SerializeField] private bool debugMode = false;

    private BattleState state;

    // Add this field to track if player input is enabled
    private bool playerInputEnabled = true;

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

    // Add this method to check for victory anytime
    public void CheckForVictory()
    {
        // Skip if we're already in a winning or losing state
        if (state == BattleState.Win || state == BattleState.Lose)
            return;

        // Check for victory
        if (FindObjectsByType<EnemyStatus>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length == 0)
        {
            state = BattleState.Win;
            DisablePlayerInput();
            gameOverController.ShowWin();
            if (debugMode) Debug.Log("[TurnManager] All enemies defeated, showing win screen");
        }
    }

    public void CheckForDefeat()
    {
        // Skip if we're already in a winning or losing state
        if (state == BattleState.Win || state == BattleState.Lose)
            return;
        // Check if the player is defeated
        if (player != null && player.TryGetComponent<CharacterStats>(out var playerStats))
        {
            if (playerStats.CurrentHealth <= 0)
            {
                state = BattleState.Lose;
                DisablePlayerInput();
                gameOverController.ShowLose();
                if (debugMode) Debug.Log("[TurnManager] Player defeated, showing lose screen");
            }
        }
        else
        {
            if (debugMode) Debug.Log("[TurnManager] Player reference is null or missing CharacterStats component");
            state = BattleState.Lose;
            DisablePlayerInput();
            gameOverController.ShowLose();
        }
    }

    // Modify BeginPlayerTurn to check for victory at the start of the turn
    private void BeginPlayerTurn()
    {
        if (debugMode) Debug.Log("[TurnManager] BeginPlayerTurn() called");
        
        // If victory was detected, don't proceed with turn setup
        if (state == BattleState.Win || state == BattleState.Lose)
            return;
            
        state = BattleState.PlayerTurn;

        playerCost.ResetToMax();
        if (cardSpawner.SpawnAndFanCards())
        {
            if (debugMode) Debug.Log("[TurnManager] SpawnAndFanCards() triggered successfully");
        }
    }

    // Modify your EndPlayerTurn method to check if input is enabled
    public void EndPlayerTurn()
    {
        if (!playerInputEnabled || state != BattleState.PlayerTurn)
            return;

        if (debugMode) Debug.Log("[TurnManager] End Player Turn");
        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyPhase());
    }

    // Modify the EnemyPhase method to use the CheckForVictory method
    private IEnumerator EnemyPhase()
    {
        var enemies = FindObjectsByType<EnemyStatus>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        if (debugMode) Debug.Log($"[TurnManager] EnemyPhase: found {enemies.Length} enemies");
        foreach (var e in enemies)
        {
            if (debugMode) Debug.Log($"[TurnManager] EnemyPhase: calling PerformAction on {e.name}");
            yield return StartCoroutine(EnemyAICombat.Instance.PerformAction(e));
            
            // Check for victory after each enemy action
            CheckForVictory();
            // Check for defeat after each enemy action
            CheckForDefeat();

            // If victory was detected, exit the loop
            if (state == BattleState.Win || state == BattleState.Lose)
                yield break;
        }

        // Tick down any element/status durations on enemies
        foreach (var enemy in FindObjectsByType<EnemyStatus>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            enemy.TickRound();

        // TODO: if you later add status effects on the player, tick them here

        // If player still survive, start a new player turn
        BeginPlayerTurn();
    }

    // Add this method to disable player input
    public void DisablePlayerInput()
    {
        playerInputEnabled = false;

        // Disable any UI elements for player input
        if (cardSpawner != null)
        {
            // You might want to add a method to CardSpawner to disable dragging
            // For now, we can just set all cards to not be interactable
            foreach (var cardRT in cardSpawner.cardRTList)
            {
                if (cardRT != null && cardRT.GetComponent<CardDrag>() != null)
                {
                    cardRT.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    cardRT.GetComponent<CanvasGroup>().interactable = false;
                }
            }
        }
    }

    // Add this Update method to your TurnManager class
    private void Update()
    {
        // Only check for victory during player's turn
        if (state == BattleState.PlayerTurn)
        {
            // Use a frame skip optimization to avoid checking every single frame
            if (Time.frameCount % 15 == 0) // Check roughly every 15 frames (about 4 times per second at 60 FPS)
            {
                CheckForVictory();
            }
        }
    }
}
