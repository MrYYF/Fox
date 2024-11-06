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

    //通过值查询对应屏幕模式
    public static FullScreenMode QuaryDisplayMode(int displayMode)
    {
        switch (displayMode)
        {
            case 0:
                return FullScreenMode.ExclusiveFullScreen; //全屏
            case 1:
                return FullScreenMode.FullScreenWindow; //无边框
            case 2:
                return FullScreenMode.Windowed; //窗口化
            default:
                return FullScreenMode.ExclusiveFullScreen;
        }
    }
    //通过值查询对应分辨率
    public static int QuaryResolutionWidth(int resolution)
    {
        switch (resolution)
        {
            case 0:
                return 800;
            case 1:
                return 1024;
            case 2:
                return 1280;
            case 3:
                return 1366;
            case 4:
                return 1600;
            case 5:
                return 1920;
            case 6:
                return 1920;
            case 7:
                return 2560;
            case 8:
                return 2560;
            case 9:
                return 3840;
            default:
                return 1920;
        }
    }
    public static int QuaryResolutionHeight(int resolution)
    {
        switch (resolution)
        {
            case 0:
                return 600;
            case 1:
                return 768;
            case 2:
                return 720;
            case 3:
                return 768;
            case 4:
                return 900;
            case 5:
                return 1080;
            case 6:
                return 1200;
            case 7:
                return 1440;
            case 8:
                return 1600;
            case 9:
                return 2160;
            default:
                return 1080;
        }
    }
}
