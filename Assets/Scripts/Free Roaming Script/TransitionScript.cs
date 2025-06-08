using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnPointId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.useCustomSpawnPosition = true;
            GameManager.Instance.targetSpawnPointId = targetSpawnPointId;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
