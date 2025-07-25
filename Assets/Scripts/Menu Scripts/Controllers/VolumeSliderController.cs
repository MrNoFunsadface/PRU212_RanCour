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
        SoundManager.Instance.MasterVolume = masterVolumeSlider.value;
        SaveVolumeSettings();
    }

    public void SetSFXVolume()
    {
        SoundManager.Instance.SFXVolume = sfxVolumeSlider.value;
        SaveVolumeSettings();
    }

    public void SetMusicVolume()
    {
        SoundManager.Instance.MusicVolume = musicVolumeSlider.value;
        SaveVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        SoundManager.Instance.MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_VOLUME);
        SoundManager.Instance.SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_VOLUME);
        SoundManager.Instance.MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
        masterVolumeSlider.value = SoundManager.Instance.MasterVolume;
        sfxVolumeSlider.value = SoundManager.Instance.SFXVolume;
        musicVolumeSlider.value = SoundManager.Instance.MusicVolume;
    }
    
    private void SaveVolumeSettings()
    {
        // Only save if SoundManager is properly initialized
        if (!SoundManager.IsInitialized)
        {
            Debug.LogWarning("Cannot save volume settings: SoundManager not initialized");
            return;
        }
        
        float masterValue = SoundManager.Instance.MasterVolume;
        float sfxValue = SoundManager.Instance.SFXVolume;
        float musicValue = SoundManager.Instance.MusicVolume;

        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterValue);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxValue);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicValue);
        PlayerPrefs.Save();
        
        Debug.Log($"Saved volume settings - Master: {masterValue}, SFX: {sfxValue}, Music: {musicValue}");
    }
}