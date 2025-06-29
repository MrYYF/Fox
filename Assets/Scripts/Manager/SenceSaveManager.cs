using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
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
        LoadSence(currentGameSceneData); // 加载第一个场景
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
        if (currentGameSceneData != null)
            StartCoroutine(UnLoadPreviousScene(gameSceneData));
    }

    // 异步加载场景
    private AsyncOperationHandle LoadSence(GameSceneDataSO gameSceneData) {
        currentGameSceneData = gameSceneData; // 更新当前场景数据
        AsyncOperationHandle<SceneInstance> asyncOperationHandle = gameSceneData.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        //asyncOperationHandle.IsDone; //是否完成
        //asyncOperationHandle.PercentComplete; //加载进度
        //asyncOperationHandle.Status; //当前加载状态（成功、失败、进行中）
        return asyncOperationHandle;
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
        if(checkPoint != currentCheckPoint) {
            if(currentCheckPoint != null)
                currentCheckPoint.isUsed = false; // 设置之前的存档点未使用
            checkPoint.isUsed = true; // 设置当前存档点为已使用
            currentCheckPoint = checkPoint;
        }
    }

    // 注册初始存档点
    public void RegisterInitialCheckPoint() {
        var initial = FindObjectsOfType<CheckPoint>()
            .FirstOrDefault(cp => cp.isInitialCheckPoint);

        if (initial != null) {
            SetCheckPoint(initial.GetComponent<CheckPoint>());
            Debug.Log("自动注册初始重生点：" + initial.name);
        }
    }

    private IEnumerator UnLoadPreviousScene(GameSceneDataSO gameSceneData) {
        // TODO:渐入
        //yield return new WaitForSeconds(fadeDuration);

        yield return currentGameSceneData.sceneReference.UnLoadScene();
        yield return LoadSence(gameSceneData);
        RegisterInitialCheckPoint(); // 注册初始存档点

        // TODO:渐出
        yield return new WaitForSeconds(fadeDuration);
        GameManager.Instance.CreatePlayer(currentCheckPoint.spawnPoint.position);
    }
}
