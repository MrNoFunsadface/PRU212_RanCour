using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private static bool initialized = false;

    // Use the same constant keys as VolumeSliderController
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const float DEFAULT_VOLUME = 1f;

    private void Awake()
    {
        if (!initialized)
        {
            // Make sure SoundManager is ready before setting volumes
            StartCoroutine(WaitForSoundManagerAndInitialize());
            initialized = true;
        }
    }
    
    private IEnumerator WaitForSoundManagerAndInitialize()
    {
        // Wait until SoundManager is properly initialized
        while (!SoundManager.IsInitialized)
        {
            Debug.Log("Waiting for SoundManager to initialize...");
            yield return null;
        }
        
        // Now it's safe to load volume settings
        LoadVolumeSettings();
        Debug.Log("AudioManager: Volume settings loaded successfully");
    }
    
    private void LoadVolumeSettings()
    {
        // Load the volume settings from PlayerPrefs
        float masterValue = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_VOLUME);
        float sfxValue = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_VOLUME);
        float musicValue = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
        
        Debug.Log($"AudioManager loading values - Master: {masterValue}, SFX: {sfxValue}, Music: {musicValue}");
        
        // Apply them to SoundManager if it's initialized
        if (SoundManager.IsInitialized)
        {
            SoundManager.MasterVolume = masterValue;
            SoundManager.SFXVolume = sfxValue;
            SoundManager.MusicVolume = musicValue;
        }
        else
        {
            Debug.LogWarning("Cannot load volume settings: SoundManager not initialized");
        }
    }
}