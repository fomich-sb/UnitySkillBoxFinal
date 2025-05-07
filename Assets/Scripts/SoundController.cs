using AK.Wwise;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public RTPC MusicVolume;
    public RTPC SFXVolume;
    public AK.Wwise.Bank globalSoundBank; // Банк с общими звуками

    private void Awake()
    {
        globalSoundBank.Load(); // Загружаем банк один раз
    }

    private void OnDestroy()
    {
        globalSoundBank.Unload(); // Выгружаем при завершении
    }
}
