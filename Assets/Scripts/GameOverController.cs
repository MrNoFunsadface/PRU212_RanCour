// GameOverController.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;   // drag in your Win UI
    [SerializeField] private GameObject losePanel;  // drag in your Lose UI
    [SerializeField] private string victoryScene; // The main game scene
    [SerializeField] private string defeatScene = "Main menu"; // The scene to return to after battle
    [SerializeField] private float autoTransitionDelay = 3f; // Optional: auto return after delay

    // Flag to mark battle outcome
    public static bool IsVictory { get; private set; }

    // Store the current wave ID
    private string currentWaveId;

    private void Start()
    {
        victoryScene = BattleTransitionData.fromSceneName; // Get the scene name from BattleTransitionData
    }

    public void ShowWin()
    {
        IsVictory = true;
        currentWaveId = BattleTransitionData.SelectedWaveId;

        // Store the wave ID and mark it as victorious
        if (!string.IsNullOrEmpty(currentWaveId))
        {
            GameManager.Instance.lastBattleWaveId = currentWaveId;
            GameManager.Instance.lastBattleResult = true;
        }

        winPanel.SetActive(true);

        // Disable inputs or other game elements as needed
        DisableGameplayElements();

        // Optional: Auto-transition after delay
        if (autoTransitionDelay > 0)
            Invoke(nameof(ReturnToMainScene), autoTransitionDelay);
    }

    public void ShowLose()
    {
        IsVictory = false;

        // Store the result in GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.lastBattleResult = false;
        }

        losePanel.SetActive(true);

        // Disable inputs or other game elements as needed
        DisableGameplayElements();

        // Optional: Auto-transition after delay
        if (autoTransitionDelay > 0)
            Invoke(nameof(ReturnToMainScene), autoTransitionDelay);
    }

    // Called by UI buttons or auto-invoked
    public void ReturnToMainScene()
    {
        // If victorious, remove the wave before returning
        if (IsVictory)
        {
            // This ensures the enemy doesn't respawn
            MobWaveDataManager.RemoveWave(currentWaveId);

            // Play transition sound
            SoundManager.Instance.PlaySound(SoundEffectType.MENUOPEN);

            // Load the victory scene
            SceneManager.LoadScene(victoryScene);

            return;
        }

        // Play transition sound
        SoundManager.Instance.PlaySound(SoundEffectType.MENUOPEN);

        // Load the defeat scene
        SceneManager.LoadScene(defeatScene);
    }

    private void DisableGameplayElements()
    {
        // Disable card dragging, player input, etc.
        // Example:
        if (TurnManager.Instance != null)
        {
            TurnManager.Instance.DisablePlayerInput();
        }
    }
}
