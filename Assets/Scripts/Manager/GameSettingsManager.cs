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
        {
            LoadSettings();
        }
        else // 如果没有设置文件，初始化为默认值
        {
            gameSettings = GameSettingsData.CreateDefaultSettings();
            ApplySettings();
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
        SyncSettings();
    }

    //重置设置
    public void ResetSettings()
    {
        gameSettings = GameSettingsData.CreateDefaultSettings();
        SyncSettings();
        SaveSettings();
    }

    //应用设置
    public void ApplySettings()
    {
        ChangeDisplayMode();
        ChangeResolution();
        ChangeVSync();
        ChangeTargetFrameRate();
        ChangeMasterVolume(masterVolumeText);
        ChangeMusicVolume(musicVolumeText);
        ChangeSFXVolume(SFXVolumeText);
        SaveSettings();
    }

    //将UI和设置同步
    public void SyncSettings()
    {
        displayMode.value = gameSettings.displayMode;
        resolution.value = gameSettings.resolution;
        VSync.isOn = gameSettings.VSync;
        frameRateLimit.value = gameSettings.frameRateLimit;

        masterVolume.value = gameSettings.masterVolume;
        musicVolume.value = gameSettings.musicVolume;
        SFXVolume.value = gameSettings.SFXVolume;
    }

    ////修改设置
    //public void ModifySettings()
    //{

    //    //视频设置修改
    //    newGameSettings.displayMode = displayMode.value;
    //    newGameSettings.resolution = resolution.value;
    //    newGameSettings.VSync = VSync.isOn;
    //    newGameSettings.frameRateLimit = frameRateLimit.value;

    //    //音频设置修改
    //    newGameSettings.masterVolume = masterVolume.value;
    //    newGameSettings.musicVolume = musicVolume.value;
    //    newGameSettings.SFXVolume = SFXVolume.value;

    //    //实时预览
    //    //AudioListener.volume = newGameSettings.masterVolume;
    //    //musicAudioSource.volume = newGameSettings.musicVolume;
    //    //SFXAudioSource.volume = newGameSettings.SFXVolume;
    //}

    //更改视频设置
    public void ChangeDisplayMode()
    {
        //设置显示模式
        Screen.SetResolution(GameSettingsData.QuaryResolutionWidth(resolution.value), GameSettingsData.QuaryResolutionHeight(resolution.value), GameSettingsData.QuaryDisplayMode(displayMode.value));
        gameSettings.displayMode = displayMode.value;
    }

    public void ChangeResolution()
    {
        //设置分辨率
        Screen.SetResolution(GameSettingsData.QuaryResolutionWidth(resolution.value), GameSettingsData.QuaryResolutionHeight(resolution.value), GameSettingsData.QuaryDisplayMode(displayMode.value));
        gameSettings.resolution = resolution.value;
    }

    //更改垂直同步
    public void ChangeVSync()
    {
        if (VSync.isOn) //开启垂直同步
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        frameRateLimit.enabled = !VSync.isOn;
        gameSettings.VSync = VSync.isOn;
    }

    //更改帧率限制
    public void ChangeTargetFrameRate()
    {
        int targetFrameRate = -1; //不限制
        switch (frameRateLimit.value)
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

        gameSettings.frameRateLimit = frameRateLimit.value;
    }

    //更改音量设置
    public void ChangeMasterVolume(Text volumeText)
    {
        AudioListener.volume = masterVolume.value;
        volumeText.text = Mathf.FloorToInt(masterVolume.value * 100).ToString();

        gameSettings.masterVolume = masterVolume.value;
    }

    public void ChangeMusicVolume(Text volumeText)
    {
        musicAudioSource.volume = musicVolume.value;
        volumeText.text = Mathf.FloorToInt(musicVolume.value * 100).ToString();
        
        gameSettings.musicVolume = musicVolume.value;
    }

    public void ChangeSFXVolume(Text volumeText)
    {
        SFXAudioSource.volume = SFXVolume.value;
        volumeText.text = Mathf.FloorToInt(SFXVolume.value * 100).ToString();

        gameSettings.SFXVolume = SFXVolume.value;
    }
}
