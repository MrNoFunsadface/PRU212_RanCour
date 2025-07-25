using System.Collections.Generic;
using UnityEngine;

//
// Summary: MobWaveDataManager is responsible for managing MobWaveData in the game.

public static class MobWaveDataManager
{
    public static Dictionary<string, MobWaveData> mobWaves = new();

    public static void AddWave(MobWaveData waveData)
    {
        if (mobWaves.ContainsKey(waveData.waveId))
        {
            Debug.LogWarning($"[MobWaveDataManager] Wave with ID {waveData.waveId} already exists. Overwriting.");
        }
        mobWaves[waveData.waveId] = waveData;
    }

    public static void RemoveWave(string waveId)
    {
        if (mobWaves.ContainsKey(waveId))
        {
            mobWaves.Remove(waveId);
        }
        else
        {
            Debug.LogWarning($"[MobWaveDataManager] Wave with ID {waveId} does not exist.");
        }
    }

    public static void GetWave(string waveId, out MobWaveData waveData)
    {
        if (mobWaves.TryGetValue(waveId, out waveData))
        {
            return;
        }
        else
        {
            Debug.LogWarning($"[MobWaveDataManager] Wave with ID {waveId} not found.");
            waveData = null;
        }
    }

    public static void GetWaveBySpawner(string spawnerId, out MobWaveData waveData)
    {
        foreach (var wave in mobWaves.Values)
        {
            if (wave.spawnerId == spawnerId)
            {
                waveData = wave;
                return;
            }
        }
        Debug.LogWarning($"[MobWaveDataManager] Wave with spawnerId {spawnerId} not found.");
        waveData = null;
    }

    public static void MarkAsVictory(string waveId)
    {
        if (mobWaves.TryGetValue(waveId, out var waveData))
        {
            waveData.isVictory = true;
            Debug.Log($"[MobWaveDataManager] Wave {waveId} marked as victorious.");

            // Optional: Save to PlayerPrefs for persistence between game sessions
            PlayerPrefs.SetInt($"Wave_{waveId}_Victorious", 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning($"[MobWaveDataManager] Cannot mark victory: Wave with ID {waveId} not found.");
        }
    }

    // Check if a wave is victorious
    public static bool IsWaveVictorious(string waveId)
    {
        // First check in-memory data
        if (mobWaves.TryGetValue(waveId, out var waveData))
        {
            return waveData.isVictory;
        }

        // If not found in memory, check PlayerPrefs (for persistence between sessions)
        return PlayerPrefs.GetInt($"Wave_{waveId}_Victorious", 0) == 1;
    }

    public static void LoadVictoryData()
    {
        foreach (var waveData in mobWaves.Values)
        {
            if (PlayerPrefs.GetInt($"Wave_{waveData.waveId}_Victorious", 0) == 1)
            {
                waveData.isVictory = true;
            }
        }
    }
}

[System.Serializable]
public class MobWaveData
{
    public string waveId;
    public string spawnerId;
    public List<EnemyWithStats> enemies;
    public bool isVictory = false;

    public MobWaveData(string waveId, string spawnerId, List<EnemyWithStats> enemies)
    {
        this.waveId = waveId;
        this.spawnerId = spawnerId;
        this.enemies = enemies;
        this.isVictory = false;
    }
}