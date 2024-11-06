using System;
using UnityEngine;

[Serializable]
public class GameSettingsData
{
    public int displayMode; //��ʾģʽ
    public int resolution; //�ֱ���
    public bool VSync; //��ֱͬ��
    public int frameRateLimit; //֡������

    public float masterVolume; //������
    public float musicVolume; //��������
    public float SFXVolume; //Ч������

    // ˽�й��캯������ֹ�ⲿֱ�Ӵ���ʵ��
    private GameSettingsData() { }

    //ȫ�ι��캯��
    public GameSettingsData(
        int displayMode = 0,
        int resolution = 5,
        bool vSync = false,
        int frameRateLimit = 0,
        float masterVolume = 1f,
        float musicVolume = 1f,
        float SFXVolume = 1f)
    {
        this.displayMode = displayMode;
        this.resolution = resolution;
        this.VSync = vSync;
        this.frameRateLimit = frameRateLimit;
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.SFXVolume = SFXVolume;
    }

    // ��������������Ĭ������
    public static GameSettingsData CreateDefaultSettings()
    {
        GameSettingsData settings = new GameSettingsData
        {
            displayMode = 0,
            resolution = 5,
            VSync = false,
            frameRateLimit = 0,
            masterVolume = 1f,
            musicVolume = 1f,
            SFXVolume = 1f,
        };
        return settings;
    }

    
}
