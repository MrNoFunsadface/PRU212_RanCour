using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    // Constants for PlayerPrefs keys
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const float DEFAULT_VOLUME = 1f;
    
    private void Start()
    {
        LoadVolumeSettings();
    }

    public void SetMasterVolume()
    {
        SoundManager.MasterVolume = masterVolumeSlider.value;
        SaveVolumeSettings();
    }

    public void SetSFXVolume()
    {
        SoundManager.SFXVolume = sfxVolumeSlider.value;
        SaveVolumeSettings();
    }

    public void SetMusicVolume()
    {
        SoundManager.MusicVolume = musicVolumeSlider.value;
        SaveVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        SoundManager.MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_VOLUME);
        SoundManager.SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_VOLUME);
        SoundManager.MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
        masterVolumeSlider.value = SoundManager.MasterVolume;
        sfxVolumeSlider.value = SoundManager.SFXVolume;
        musicVolumeSlider.value = SoundManager.MusicVolume;
    }
    
    private void SaveVolumeSettings()
    {
        // Only save if SoundManager is properly initialized
        if (!SoundManager.IsInitialized)
        {
            Debug.LogWarning("Cannot save volume settings: SoundManager not initialized");
            return;
        }
        
        float masterValue = SoundManager.MasterVolume;
        float sfxValue = SoundManager.SFXVolume;
        float musicValue = SoundManager.MusicVolume;

        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterValue);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxValue);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicValue);
        PlayerPrefs.Save();
        
        Debug.Log($"Saved volume settings - Master: {masterValue}, SFX: {sfxValue}, Music: {musicValue}");
    }
}