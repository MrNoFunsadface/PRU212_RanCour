using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering;

public class EnviromentController : MonoBehaviour
{
    private FogBarrel fogBarrel;

    private EnemyAI[] enemies;

    public void UpdateEnvironment()
    {
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
