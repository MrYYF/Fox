using System.IO;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager GameSettingsManagerInstance;

    [Header("Video Setting")]
    public Dropdown displayMode;
    public Dropdown resolution;
    public Toggle VSync;
    public Dropdown frameRateLimit;

    [Header("Audio Setting")]
    public Slider masterVolume;
    public Text masterVolumeText;
    [Space]
    public Slider musicVolume;
    public Text musicVolumeText;
    public AudioSource musicAudioSource;
    [Space]
    public Slider SFXVolume;
    public Text SFXVolumeText;
    public AudioSource SFXAudioSource;


    private string filePath;
    private GameSettingsData gameSettings;
    private GameSettingsData newGameSettings;

    private void Awake()
    {
        if (GameSettingsManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        GameSettingsManagerInstance = this;
    }

    void Start()
    {
        filePath = Application.dataPath + "/Data/settings.json"; // 设置存储路径 /Assets/Data/settings.json

        if (File.Exists(filePath)) // 如果文件存在，读取设置
            LoadSettings();
        else // 如果没有设置文件，初始化为默认值
        {
            gameSettings = GameSettingsData.CreateDefaultSettings();
            ApplySettings();
            SaveSettings();
        }
    }
    
    // 保存设置
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(gameSettings, true); // 转换为 JSON 字符串
        File.WriteAllText(filePath, json); // 将设置写入文件
    }

    // 加载设置
    public void LoadSettings()
    {
        string json = File.ReadAllText(filePath); // 读取文件内容
        gameSettings = JsonUtility.FromJson<GameSettingsData>(json); // 解析 JSON

        displayMode.value = gameSettings.displayMode;
        resolution.value = gameSettings.resolution;
        VSync.isOn=gameSettings.VSync;
        frameRateLimit.value = gameSettings.frameRateLimit;

        masterVolume.value = gameSettings.masterVolume;
        musicVolume.value = gameSettings.musicVolume;
        SFXVolume.value = gameSettings.SFXVolume;
        masterVolumeText.text = Mathf.FloorToInt(gameSettings.masterVolume * 100).ToString();
        musicVolumeText.text = Mathf.FloorToInt(gameSettings.musicVolume * 100).ToString();
        SFXVolumeText.text = Mathf.FloorToInt(gameSettings.SFXVolume * 100).ToString();

        ApplySettings();
    }

    //应用设置
    public void ApplySettings()
    {
        ChangeDisplayModeAndResolution();
        ChangeTargetFrameRate();
        ChangeVolume();
    }

    //重置设置
    public void ResetSettings()
    {
        gameSettings = GameSettingsData.CreateDefaultSettings();
        ApplySettings();
        SaveSettings();
    }

    //修改设置
    public void ModifySettings()
    {
        //视频设置修改
        gameSettings.displayMode = displayMode.value;
        gameSettings.resolution = resolution.value;
        gameSettings.VSync = VSync.isOn;
        gameSettings.frameRateLimit = frameRateLimit.value;

        //音频设置修改
        gameSettings.masterVolume = masterVolume.value;
        gameSettings.musicVolume = musicVolume.value;
        gameSettings.SFXVolume = SFXVolume.value;
        masterVolumeText.text = Mathf.FloorToInt(masterVolume.value * 100).ToString();
        musicVolumeText.text = Mathf.FloorToInt(musicVolume.value * 100).ToString();
        SFXVolumeText.text = Mathf.FloorToInt(SFXVolume.value * 100).ToString();

        //实时预览
        ChangeVolume();
    }

    //更改视频设置
    public void ChangeDisplayModeAndResolution()
    {
        FullScreenMode windowMode = FullScreenMode.Windowed;
        int resolutionWidth =1920;
        int resolutionHeight =1080;

        //设置显示模式
        switch (gameSettings.displayMode)
        {
            case 0:
                windowMode = FullScreenMode.ExclusiveFullScreen; //全屏
                break;
            case 1:
                windowMode = FullScreenMode.FullScreenWindow; //无边框
                break;
            case 2:
                windowMode = FullScreenMode.Windowed; //窗口化
                break;
        }

        //设置分辨率
        switch (gameSettings.resolution)
        {
            case 0:
                {
                    resolutionWidth = 800;
                    resolutionHeight = 600;
                    break;
                }
            case 1:
                {
                    resolutionWidth = 1024;
                    resolutionHeight = 768;
                    break;
                }
            case 2:
                {
                    resolutionWidth = 1280;
                    resolutionHeight = 720;
                    break;
                }
            case 3:
                {
                    resolutionWidth = 1366;
                    resolutionHeight = 768;
                    break;
                }
            case 4:
                {
                    resolutionWidth = 1600;
                    resolutionHeight = 900;
                    break;
                }
            case 5:
                {
                    resolutionWidth = 1920;
                    resolutionHeight = 1080;
                    break;
                }
            case 6:
                {
                    resolutionWidth = 1920;
                    resolutionHeight = 1200;
                    break;
                }
            case 7:
                {
                    resolutionWidth = 2560;
                    resolutionHeight = 1440;
                    break;
                }
            case 8:
                {
                    resolutionWidth = 2560;
                    resolutionHeight = 1600;
                    break;
                }
            case 9:
                {
                    resolutionWidth = 3840;
                    resolutionHeight = 2160;
                    break;
                }

        }

        Screen.SetResolution(resolutionWidth, resolutionHeight, windowMode);
    }

    //更改帧率限制
    public void ChangeTargetFrameRate()
    {
        int targetFrameRate = -1; //不限制

        switch (gameSettings.frameRateLimit)
        {

            case 0:
                {
                    targetFrameRate = 30;
                    break;
                }
            case 1:
                {
                    targetFrameRate = 60;
                    break;
                }
            case 2:
                {
                    targetFrameRate = 120;
                    break;
                }
            case 3:
                {
                    targetFrameRate = 144;
                    break;
                }
            case 4:
                {
                    targetFrameRate = -1;
                    break;
                }

        }

        Application.targetFrameRate = targetFrameRate;
    }

    //更改音量设置
    public void ChangeVolume()
    {
        AudioListener.volume = gameSettings.masterVolume;
        musicAudioSource.volume = gameSettings.musicVolume;
        SFXAudioSource.volume = gameSettings.SFXVolume;
    }
}
