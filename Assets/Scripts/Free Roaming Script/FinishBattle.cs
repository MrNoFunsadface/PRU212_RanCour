using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBattle : MonoBehaviour
{
    public void OnFinishBattleClicked()
    {
        string enemyId = BattleStateTracker.enemyIdToRemove;

        if (!string.IsNullOrEmpty(enemyId))
        {
            if (!GameManager.Instance.defeatedEnemyIds.Contains(enemyId))
            {
                GameManager.Instance.defeatedEnemyIds.Add(enemyId);
                Debug.Log($"Added defeated enemy to list: {enemyId}");
            }
        }
        else
        {
            Debug.LogWarning("FinishBattle: enemyIdToRemove is null or empty!");
        }

        // Clear BattleStateTracker so it doesn't carry over wrongly
        BattleStateTracker.enemyIdToRemove = null;

        SceneManager.LoadScene("Scene1"); // Your world scene name
    }
}
