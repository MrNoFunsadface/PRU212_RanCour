using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector3 playerReturnPosition;

    // Indicates if returning from a battle scene, used for restoring player's location in PlayerController
    public bool isReturningFromBattle = false;

    public bool useCustomSpawnPosition = false;
    public string targetSpawnPointId = "";

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
}
