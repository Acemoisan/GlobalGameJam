/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LabeledData<T>
{
    public T Data;
    private string _label { get; }

    public LabeledData(string label)
    {
        this._label = label;
    }

}

[CreateAssetMenu(fileName = "PreferencesSystem", menuName = "Scriptable Objects/Managers/Preferences Manager")]
public class PreferencesSystem : ScriptableObject
{
    [Header("Preferences")]
    public float volumeMaster;
    public float volumeMusic;
    public float volumeSound;
    public float volumeAmbient;

    [Header("Dependencies")]
    public AudioMixer mixer;


    // Preference Keys
    public const string VOLUME_MASTER_KEY = "volume_master";
    public const string VOLUME_MUSIC_KEY = "volume_music";
    public const string VOLUME_SOUND_KEY = "volume_sound";
    public const string VOLUME_AMBIENT_KEY = "volume_ambient";


    public void DeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void LoadPreferences()
    {
        this.LoadVolume();
    }

    
    public void VolumeMasterChanged(float volume) { VolumeChanged(VOLUME_MASTER_KEY, volume); }
    public void VolumeMusicChanged(float volume) { VolumeChanged(VOLUME_MUSIC_KEY, volume); }
    public void VolumeSoundChanged(float volume) { VolumeChanged(VOLUME_SOUND_KEY, volume); }
    public void VolumeAmbientChanged(float volume) { VolumeChanged(VOLUME_AMBIENT_KEY, volume); }
    public void VolumeChanged(string volumeType, float volume)
    {
        this.SaveVolume(volumeType, volume);
        this.LoadVolume(volumeType);
    }

    private void SaveVolume(string volumeType, float volume)
    {
        if (volumeType == VOLUME_MASTER_KEY) PlayerPrefs.SetFloat(VOLUME_MASTER_KEY, volume);
        if (volumeType == VOLUME_MUSIC_KEY) PlayerPrefs.SetFloat(VOLUME_MUSIC_KEY, volume);
        if (volumeType == VOLUME_SOUND_KEY) PlayerPrefs.SetFloat(VOLUME_SOUND_KEY, volume);
        if (volumeType == VOLUME_AMBIENT_KEY) PlayerPrefs.SetFloat(VOLUME_AMBIENT_KEY, volume);
    }

    private void LoadVolume()
    {
        this.volumeMaster = PlayerPrefs.GetFloat(VOLUME_MASTER_KEY, 1f);
        this.volumeMusic = PlayerPrefs.GetFloat(VOLUME_MUSIC_KEY, 1f);
        this.volumeSound= PlayerPrefs.GetFloat(VOLUME_SOUND_KEY, 1f);
        this.volumeAmbient = PlayerPrefs.GetFloat(VOLUME_AMBIENT_KEY, 1f);

        this.mixer?.SetFloat("MasterVolume", Mathf.Log10(this.volumeMaster) * 20);
        this.mixer?.SetFloat("MusicVolume", Mathf.Log10(this.volumeMusic) * 20);
        this.mixer?.SetFloat("SoundVolume", Mathf.Log10(this.volumeSound) * 20);
        this.mixer?.SetFloat("AmbientVolume", Mathf.Log10(this.volumeAmbient) * 20);
    }

    private void LoadVolume(string volumeType)
    {
        if (volumeType == VOLUME_MASTER_KEY)
        {
            this.volumeMaster = PlayerPrefs.GetFloat(VOLUME_MASTER_KEY, 1f);
            this.mixer?.SetFloat("MasterVolume", Mathf.Log10(this.volumeMaster) * 20);
        }
        
        if (volumeType == VOLUME_MUSIC_KEY)
        {
            this.volumeMusic = PlayerPrefs.GetFloat(VOLUME_MUSIC_KEY, 1f);
            this.mixer?.SetFloat("MusicVolume", Mathf.Log10(this.volumeMusic) * 20);
        }

        if (volumeType == VOLUME_SOUND_KEY)
        {
            this.volumeSound = PlayerPrefs.GetFloat(VOLUME_SOUND_KEY, 1f);
            this.mixer?.SetFloat("SoundVolume", Mathf.Log10(this.volumeSound) * 20);
        }

        if (volumeType == VOLUME_AMBIENT_KEY)
        {
            this.volumeAmbient= PlayerPrefs.GetFloat(VOLUME_AMBIENT_KEY, 1f);
            this.mixer?.SetFloat("AmbientVolume", Mathf.Log10(this.volumeAmbient) * 20);
        }
    }
    
}
