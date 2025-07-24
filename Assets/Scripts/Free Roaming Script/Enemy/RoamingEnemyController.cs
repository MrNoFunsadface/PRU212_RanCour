using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

//
// Summary:
//     RoamingEnemyController handles the interaction with the player when they enter
//     the enemy's trigger area, as well as passing into the battle scene wave ID.
public class RoamingEnemyController : MonoBehaviour
{
    public string waveId;
    public string spawnerId;

    // private fields
    private readonly float iFrameDuration = 1f;
    private float iFrameTimer = 0f;
    private bool canCollide = false;

    private PlayableDirector TransitionDirector;

    public Vector2 enemyReturnPosition; // Position to return to after the battle

    private void Start()
    {
        iFrameTimer = 0f;
        canCollide = false;
        TransitionDirector = FindFirstObjectByType<PlayableDirector>();
    }

    private void Update()
    {
        if (!canCollide)
        {
            iFrameTimer += Time.deltaTime;
            if (iFrameTimer >= iFrameDuration)
            {
                canCollide = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D player)
    {
        iFrameTimer += Time.deltaTime;
        if (player.collider.CompareTag("Player") && canCollide)
        {
            GameManager.Instance.isReturningFromBattle = true; // Indicate returning from battle to spawn at the correct position
            GameManager.Instance.isEnemyReturningFromBattle = true;

            GameManager.Instance.playerReturnPosition = player.transform.position; // Save player's position
            GameManager.Instance.enemyReturnPosition = transform.position; // Save enemy's position

            GameManager.Instance.lastBattleSpawnerId = spawnerId; // Save the last battle spawner ID

            // Save waveId to a static or persistent object for the battle scene
            BattleTransitionData.SelectedWaveId = waveId;

            // Play transition effect and load the battle scene
            StartCoroutine(TransitionToScene("BattleScene")); // Replace "BattleScene" with your actual battle scene name
        }
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        // Play transition effect if available
        if (TransitionDirector != null)
        {
            TransitionDirector.Play();
        }
        else
        {
            Debug.LogWarning("[RoamingEnemyController] TransitionDirector is not assigned. Using default scene load.");
        }
        // Wait for the transition to complete
        yield return new WaitForSeconds(0.7f);
        // Load the battle scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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