using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering;

public class EnviromentController : MonoBehaviour
{
    private FogBarrel fogBarrel;

    private EnemyAI[] enemies;

    public void UpdateEnvironment()
    {
        Debug.Log("Update environment called");
        if (PlayerPrefs.GetInt("AlchemyBookVolume1_Collected", 0) == 1)
        {
            if (fogBarrel != null) fogBarrel.Hide();
            foreach (var enemy in enemies)
            {
                enemy.SetToRoaming();
            }
            Debug.Log("AlchemyBookVolume1 found, loaded all enemies and hidden FogBarrel");
        }
    }

    private void Start()
    {
        fogBarrel = FindFirstObjectByType<FogBarrel>();
        if (fogBarrel == null)
        {
            Debug.Log("FogBarrel not found in the scene.");
        }
        else
        {
            Debug.Log("FogBarrel found in the scene.");
        }

        enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            enemy.SetToDespawned();
        }

        UpdateEnvironment();
    }
}
