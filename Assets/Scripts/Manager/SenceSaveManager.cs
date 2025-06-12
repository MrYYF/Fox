using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ���������ݴ洢������
/// </summary>
[DefaultExecutionOrder(-1)]
public class SenceSaveManager : Singleton<SenceSaveManager>
{
    [Header("�¼�����")]
    [Tooltip("�����л��¼�")] public SenceLoadEventSO senceLoadEvent; //�����л��¼�SO

    [Header("��������")]
    private CheckPoint currentCheckPoint; // ��ǰ�浵��

    [Header("���ݴ洢����")]
    // TODO: ��SO�洢����
    public bool temporarySave; //��ʱ�浵
    PlayerInputControl playerInputControl; //����ϵͳ

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        playerInputControl = new PlayerInputControl();
        playerInputControl.Gameplay.Restart.started += ReloadSence;
        
    }
    void OnEnable() {
        playerInputControl?.Enable();
        senceLoadEvent.OnEventRaised += OnSenceLoadEvent;
    }

    void OnDisable() {
        playerInputControl?.Disable();
        senceLoadEvent.OnEventRaised -= OnSenceLoadEvent;
    }

    private void OnSenceLoadEvent(GameSceneDataSO gameSceneData) {
        LoadSence(gameSceneData); // �첽���س���
        SetCheckPoint(gameSceneData.CheckPoint); // ���õ�ǰ�浵��
    }

    // �첽���س���
    private void LoadSence(GameSceneDataSO gameSceneData) {
        Addressables.LoadSceneAsync(gameSceneData.sceneReference, LoadSceneMode.Additive);
    }

    // ���¼��ص�ǰ����
    private void ReloadSence(InputAction.CallbackContext context) {
        // ɾ����ǰ���
        GameManager.Instance.DestroyPlayer();
        // �ڵ�ǰ�浵������
        GameManager.Instance.CreatePlayer(currentCheckPoint.spawnPoint.position);
    }

    // ���ü���
    internal void SetCheckPoint(CheckPoint checkPoint) {
        if(currentCheckPoint != null && checkPoint != currentCheckPoint) {
            currentCheckPoint.isUsed = false; // ����֮ǰ�Ĵ浵��δʹ��
            checkPoint.isUsed = true; // ���õ�ǰ�浵��Ϊ��ʹ��
            currentCheckPoint = checkPoint;
        }
    }


}
