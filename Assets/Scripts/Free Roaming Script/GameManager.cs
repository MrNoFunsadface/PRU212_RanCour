using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector3 playerReturnPosition;
    public List<string> defeatedEnemyIds = new();

    // Indicates if returning from a battle scene, used for restoring player's location in PlayerController
    public bool isReturningFromBattle = false;

    public bool useCustomSpawnPosition = false;
    public string targetSpawnPointId = "";

    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            playerReturnPosition = player.transform.position;
        }
    }
}
