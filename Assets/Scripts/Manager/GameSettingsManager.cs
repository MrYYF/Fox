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
        {
            LoadSettings();
        }
        else // ���û�������ļ�����ʼ��ΪĬ��ֵ
        {
            gameSettings = GameSettingsData.CreateDefaultSettings();
            ApplySettings();
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
        SyncSettings();
    }

    //��������
    public void ResetSettings()
    {
        gameSettings = GameSettingsData.CreateDefaultSettings();
        SyncSettings();
        SaveSettings();
    }

    //Ӧ������
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

    //��UI������ͬ��
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

    ////�޸�����
    //public void ModifySettings()
    //{

    //    //��Ƶ�����޸�
    //    newGameSettings.displayMode = displayMode.value;
    //    newGameSettings.resolution = resolution.value;
    //    newGameSettings.VSync = VSync.isOn;
    //    newGameSettings.frameRateLimit = frameRateLimit.value;

    //    //��Ƶ�����޸�
    //    newGameSettings.masterVolume = masterVolume.value;
    //    newGameSettings.musicVolume = musicVolume.value;
    //    newGameSettings.SFXVolume = SFXVolume.value;

    //    //ʵʱԤ��
    //    //AudioListener.volume = newGameSettings.masterVolume;
    //    //musicAudioSource.volume = newGameSettings.musicVolume;
    //    //SFXAudioSource.volume = newGameSettings.SFXVolume;
    //}

    //������Ƶ����
    public void ChangeDisplayMode()
    {
        //������ʾģʽ
        Screen.SetResolution(GameSettingsData.QuaryResolutionWidth(resolution.value), GameSettingsData.QuaryResolutionHeight(resolution.value), GameSettingsData.QuaryDisplayMode(displayMode.value));
        gameSettings.displayMode = displayMode.value;
    }

    public void ChangeResolution()
    {
        //���÷ֱ���
        Screen.SetResolution(GameSettingsData.QuaryResolutionWidth(resolution.value), GameSettingsData.QuaryResolutionHeight(resolution.value), GameSettingsData.QuaryDisplayMode(displayMode.value));
        gameSettings.resolution = resolution.value;
    }

    //���Ĵ�ֱͬ��
    public void ChangeVSync()
    {
        if (VSync.isOn) //������ֱͬ��
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        frameRateLimit.enabled = !VSync.isOn;
        gameSettings.VSync = VSync.isOn;
    }

    //����֡������
    public void ChangeTargetFrameRate()
    {
        int targetFrameRate = -1; //������
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

    //������������
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
