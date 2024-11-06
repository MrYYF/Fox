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
        filePath = Application.dataPath + "/Data/settings.json"; // ���ô洢·�� /Assets/Data/settings.json

        if (File.Exists(filePath)) // ����ļ����ڣ���ȡ����
            LoadSettings();
        else // ���û�������ļ�����ʼ��ΪĬ��ֵ
        {
            gameSettings = GameSettingsData.CreateDefaultSettings();
            ApplySettings();
            SaveSettings();
        }
    }
    
    // ��������
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(gameSettings, true); // ת��Ϊ JSON �ַ���
        File.WriteAllText(filePath, json); // ������д���ļ�
    }

    // ��������
    public void LoadSettings()
    {
        string json = File.ReadAllText(filePath); // ��ȡ�ļ�����
        gameSettings = JsonUtility.FromJson<GameSettingsData>(json); // ���� JSON

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

    //Ӧ������
    public void ApplySettings()
    {
        ChangeDisplayModeAndResolution();
        ChangeTargetFrameRate();
        ChangeVolume();
    }

    //��������
    public void ResetSettings()
    {
        gameSettings = GameSettingsData.CreateDefaultSettings();
        ApplySettings();
        SaveSettings();
    }

    //�޸�����
    public void ModifySettings()
    {
        //��Ƶ�����޸�
        gameSettings.displayMode = displayMode.value;
        gameSettings.resolution = resolution.value;
        gameSettings.VSync = VSync.isOn;
        gameSettings.frameRateLimit = frameRateLimit.value;

        //��Ƶ�����޸�
        gameSettings.masterVolume = masterVolume.value;
        gameSettings.musicVolume = musicVolume.value;
        gameSettings.SFXVolume = SFXVolume.value;
        masterVolumeText.text = Mathf.FloorToInt(masterVolume.value * 100).ToString();
        musicVolumeText.text = Mathf.FloorToInt(musicVolume.value * 100).ToString();
        SFXVolumeText.text = Mathf.FloorToInt(SFXVolume.value * 100).ToString();

        //ʵʱԤ��
        ChangeVolume();
    }

    //������Ƶ����
    public void ChangeDisplayModeAndResolution()
    {
        FullScreenMode windowMode = FullScreenMode.Windowed;
        int resolutionWidth =1920;
        int resolutionHeight =1080;

        //������ʾģʽ
        switch (gameSettings.displayMode)
        {
            case 0:
                windowMode = FullScreenMode.ExclusiveFullScreen; //ȫ��
                break;
            case 1:
                windowMode = FullScreenMode.FullScreenWindow; //�ޱ߿�
                break;
            case 2:
                windowMode = FullScreenMode.Windowed; //���ڻ�
                break;
        }

        //���÷ֱ���
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

    //����֡������
    public void ChangeTargetFrameRate()
    {
        int targetFrameRate = -1; //������

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

    //������������
    public void ChangeVolume()
    {
        AudioListener.volume = gameSettings.masterVolume;
        musicAudioSource.volume = gameSettings.musicVolume;
        SFXAudioSource.volume = gameSettings.SFXVolume;
    }
}
