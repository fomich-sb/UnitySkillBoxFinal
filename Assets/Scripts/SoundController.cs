using AK.Wwise;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public RTPC MusicVolume;
    public RTPC SFXVolume;
    public AK.Wwise.Bank globalSoundBank; // ���� � ������ �������

    private void Awake()
    {
        globalSoundBank.Load(); // ��������� ���� ���� ���
    }

    private void OnDestroy()
    {
        globalSoundBank.Unload(); // ��������� ��� ����������
    }
}
