using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector2 playerReturnPosition; // Position to return to after a battle
    public Vector2 enemyReturnPosition; // Position to return to for the enemy after a battle

    public string lastBattleSpawnerId; // Store the spawner ID from the last battle
    public string lastBattleWaveId; // Store the wave ID from the last battle
    public bool lastBattleResult; // True if player won, false if lost

    // Indicates if returning from a battle scene, used for restoring player's location in PlayerController
    public bool isReturningFromBattle = false;
    public bool isEnemyReturningFromBattle = false;

    public bool useCustomSpawnPosition = false;
    public string targetSpawnPointId = "";

    public string fromSceneName = ""; // Name of the scene we are returning from, used for custom spawn points

    public SoundTrackList soundTrack; // Sound track for the roaming enemies

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            MobWaveDataManager.LoadVictoryData(); // Initialize the MobWaveDataManager
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
