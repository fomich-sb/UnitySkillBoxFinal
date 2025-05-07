using AK.Wwise;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

public class Settings : MonoBehaviour
{
    [Header("«вук")]
    [Range(0, 1)] public float effectVolume = 0.6f;
    [Range(0, 1)] public float musicVolume = 0.4f;

    [Header("”правление")]
    [Range(0.1f, 0.3f)] public float mouseSensitivity = 0.2f;

    [Header("Ёлементы управлени€")]
    [SerializeField] private Slider effectVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider mouseSensitivitySlider;

    [Inject] private SoundController soundController;

    public void SaveSettings()
    {
        effectVolume = effectVolumeSlider.value;
        musicVolume = musicVolumeSlider.value;
        mouseSensitivity = mouseSensitivitySlider.value;

        PlayerPrefs.SetFloat("effectVolume", effectVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivity);

        setSoundVolumes();
    }

    public void LoadSettings()
    {
        effectVolume = PlayerPrefs.GetFloat("effectVolume", effectVolume);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", musicVolume);
        mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", mouseSensitivity);

        effectVolumeSlider.value = effectVolume;
        musicVolumeSlider.value = musicVolume;
        mouseSensitivitySlider.value = mouseSensitivity;

        setSoundVolumes();
    }

    private void setSoundVolumes()
    {
        soundController.SFXVolume.SetGlobalValue(effectVolume * 100f);
        soundController.MusicVolume.SetGlobalValue(musicVolume * 100f);
    }
}
