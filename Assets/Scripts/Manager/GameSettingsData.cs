using System;
using UnityEngine;

[Serializable]
public class GameSettingsData
{
    public int displayMode; //显示模式
    public int resolution; //分辨率
    public bool VSync; //垂直同步
    public int frameRateLimit; //帧率限制

    public float masterVolume; //主音量
    public float musicVolume; //音乐音量
    public float SFXVolume; //效果音量

    // 私有构造函数，防止外部直接创建实例
    private GameSettingsData() { }

    //全参构造函数
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

    // 工厂方法来创建默认设置
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
