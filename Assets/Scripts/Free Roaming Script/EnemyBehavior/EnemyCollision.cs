using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollision : MonoBehaviour
{
    public string enemyId; // Assign a unique ID per enemy (e.g. "enemy1")

    private void OnCollisionEnter2D(Collision2D collision) // Corrected signature
    {
        if (collision.collider.CompareTag("Player")) // Adjusted to use collision.collider
        {
            GameManager.Instance.isReturningFromBattle = true;

            // Add enemyId to the defeated list if not already there
            if (!GameManager.Instance.defeatedEnemyIds.Contains(enemyId))
            {
                GameManager.Instance.defeatedEnemyIds.Add(enemyId);
            }

            SceneManager.LoadScene("BattleScene");
        }
    }
}
