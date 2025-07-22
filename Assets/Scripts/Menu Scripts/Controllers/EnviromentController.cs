using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EnviromentController : MonoBehaviour
{
    private FogBarrel fogBarrel;

    private EnemyAI[] enemies;

    public void UpdateEnvironment()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.GetInt("AlchemyBookVolume1_Collected", 0) == 1)
        {
            if (fogBarrel != null) fogBarrel.Hide();
            foreach (var enemy in enemies)
            {
                enemy.SetToRoaming();
            }
        }
    }

    private void Start()
    {
        fogBarrel = FindFirstObjectByType<FogBarrel>();
        enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            enemy.SetToDespawned();
        }

        UpdateEnvironment();
    }
}
