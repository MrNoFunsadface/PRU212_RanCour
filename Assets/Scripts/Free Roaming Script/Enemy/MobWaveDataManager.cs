using System.Collections.Generic;
using UnityEngine;

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
}

[System.Serializable]
public class MobWaveData
{
    public string waveId;
    public string spawnerId;
    public List<EnemyWithStats> enemies;

    public MobWaveData(string waveId, string spawnerId, List<EnemyWithStats> enemies)
    {
        this.waveId = waveId;
        this.spawnerId = spawnerId;
        this.enemies = enemies;
    }
}