using System;
using System.Collections;
using System.Linq;
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
    [Tooltip("场景切换事件")] public SceneLoadEventSO senceLoadEvent;

    [Header("场景管理")]
    [Tooltip("第一个场景数据")]public GameSceneDataSO firstGameSceneData;
    [Tooltip("当前场景数据")]public GameSceneDataSO currentGameSceneData;
    [Tooltip("下一个加载场景数据")]public GameSceneDataSO nextGameSceneData;
    public CheckPoint currentCheckPoint; // 当前存档点

    [Header("数据存储管理")]
    // TODO: 用SO存储数据
    public bool temporarySave; //临时存档
    PlayerInputControl playerInputControl; //输入系统

    public float fadeDuration = 1.0f;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        playerInputControl = new PlayerInputControl();
        playerInputControl.Gameplay.Restart.started += ReloadSence;
        currentGameSceneData = firstGameSceneData; // 初始化当前场景数据为第一个场景数据
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
        StartCoroutine(UnLoadPreviousScene());
        
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

    private IEnumerator UnLoadPreviousScene() {
        // TODO:渐入渐出

        yield return new WaitForSeconds(fadeDuration);
        Debug.Log("进入卸载场景流程currentGameSceneData${}", currentGameSceneData);
        if (currentGameSceneData != null)
            yield return currentGameSceneData.sceneReference.UnLoadScene();

        LoadSence(nextGameSceneData);
        //CheckPoint initialCheckPoint = FindObjectsOfType<CheckPoint>()
        //    .FirstOrDefault(cp => cp.isInitialCheckPoint);
        //if (initialCheckPoint != null) {
        //    SetCheckPoint(initialCheckPoint); // 设置当前存档点
        //}
    }
}
