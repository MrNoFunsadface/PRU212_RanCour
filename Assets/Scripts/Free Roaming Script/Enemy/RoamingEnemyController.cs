using UnityEngine;

//
// Summary:
//     RoamingEnemyController handles the interaction with the player when they enter
//     the enemy's trigger area, as well as passing into the battle scene wave ID.
public class RoamingEnemyController : MonoBehaviour
{
    public string waveId;

    private void OnCollisionEnter2D(Collision2D player)
    {
        if (player.collider.CompareTag("Player"))
        {
            GameManager.Instance.isReturningFromBattle = true; // Indicate returning from battle to spawn at the correct position
            // Save waveId to a static or persistent object for the battle scene
            BattleTransitionData.SelectedWaveId = waveId;
            // Load battle scene (implement your own scene loading)
            UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
        }
    }

    // Call this when the wave is defeated
    public void OnWaveDefeated()
    {
        MobWaveDataManager.RemoveWave(waveId);
        Destroy(gameObject);
    }
}

public static class BattleTransitionData
{
    public static string SelectedWaveId { get; set; } = string.Empty; // Static property to hold the selected wave ID
}