using System.Collections;
using UnityEngine;

public class SceneRestore : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null; // wait a frame to let enemies spawn

        // Move player to return position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = GameManager.Instance.playerReturnPosition;
        }

        if (GameManager.Instance.defeatedEnemyIds.Count > 0)
        {
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemies)
            {
                EnemyCollision ec = enemy.GetComponent<EnemyCollision>();
                if (ec != null && GameManager.Instance.defeatedEnemyIds.Contains(ec.enemyId))
                {
                    enemy.SetActive(false);
                    Debug.Log($"Disabled enemy: {ec.enemyId}");
                }
            }

            // Optional: clear the list if you want these disables to happen only once
            // GameManager.Instance.defeatedEnemyIds.Clear();
        }
    }
}
