using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景、数据存储管理类
/// </summary>
[DefaultExecutionOrder(-1)]
public class SenceSaveManager : Singleton<SenceSaveManager>
{
    [Header("事件监听")]
    [Tooltip("场景切换事件")] public SenceLoadEventSO senceLoadEvent; //场景切换事件SO

    [Header("场景管理")]
    private CheckPoint currentCheckPoint; // 当前存档点

    [Header("数据存储管理")]
    // TODO: 用SO存储数据
    public bool temporarySave; //临时存档
    PlayerInputControl playerInputControl; //输入系统

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
        LoadSence(gameSceneData); // 异步加载场景
        SetCheckPoint(gameSceneData.CheckPoint); // 设置当前存档点
    }

    // 异步加载场景
    private void LoadSence(GameSceneDataSO gameSceneData) {
        Addressables.LoadSceneAsync(gameSceneData.sceneReference, LoadSceneMode.Additive);
    }

    // 重新加载当前场景
    private void ReloadSence(InputAction.CallbackContext context) {
        // 删除当前玩家
        GameManager.Instance.DestroyPlayer();
        // 在当前存档点重生
        GameManager.Instance.CreatePlayer(currentCheckPoint.spawnPoint.position);
    }

    // 设置检查点
    internal void SetCheckPoint(CheckPoint checkPoint) {
        if(currentCheckPoint != null && checkPoint != currentCheckPoint) {
            currentCheckPoint.isUsed = false; // 设置之前的存档点未使用
            checkPoint.isUsed = true; // 设置当前存档点为已使用
            currentCheckPoint = checkPoint;
        }
    }


}
