using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    public string spawnPointId;

    private void Start()
    {
        StartCoroutine(HandleSpawn());
    }

    private IEnumerator HandleSpawn()
    {
        // Wait 1 frame to ensure player exists
        yield return null;

        if (GameManager.Instance != null &&
            GameManager.Instance.useCustomSpawnPosition &&
            GameManager.Instance.targetSpawnPointId == spawnPointId)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = transform.position;
            }

            GameManager.Instance.useCustomSpawnPosition = false;
            GameManager.Instance.targetSpawnPointId = "";
        }
    }
}
