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
/// ���������ݴ洢������
/// </summary>
[DefaultExecutionOrder(-1)]
public class SenceSaveManager : Singleton<SenceSaveManager>
{
    [Header("�¼�����")]
    [Tooltip("�����л��¼�")] public SceneLoadEventSO senceLoadEvent;

    [Header("��������")]
    [Tooltip("��һ����������")]public GameSceneDataSO firstGameSceneData;
    [Tooltip("��ǰ��������")]public GameSceneDataSO currentGameSceneData;
    public CheckPoint currentCheckPoint; // ��ǰ�浵��

    [Header("���ݴ洢����")]
    // TODO: ��SO�洢����
    public bool temporarySave; //��ʱ�浵
    PlayerInputControl playerInputControl; //����ϵͳ

    public float fadeDuration = 1.0f;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        playerInputControl = new PlayerInputControl();
        playerInputControl.Gameplay.Restart.started += ReloadSence;
        currentGameSceneData = firstGameSceneData; // ��ʼ����ǰ��������Ϊ��һ����������
        LoadSence(currentGameSceneData); // ���ص�һ������
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

    // �첽���س���
    private AsyncOperationHandle LoadSence(GameSceneDataSO gameSceneData) {
        currentGameSceneData = gameSceneData; // ���µ�ǰ��������
        AsyncOperationHandle<SceneInstance> asyncOperationHandle = gameSceneData.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        //asyncOperationHandle.IsDone; //�Ƿ����
        //asyncOperationHandle.PercentComplete; //���ؽ���
        //asyncOperationHandle.Status; //��ǰ����״̬���ɹ���ʧ�ܡ������У�
        return asyncOperationHandle;
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
        if(checkPoint != currentCheckPoint) {
            if(currentCheckPoint != null)
                currentCheckPoint.isUsed = false; // ����֮ǰ�Ĵ浵��δʹ��
            checkPoint.isUsed = true; // ���õ�ǰ�浵��Ϊ��ʹ��
            currentCheckPoint = checkPoint;
        }
    }

    // ע���ʼ�浵��
    public void RegisterInitialCheckPoint() {
        var initial = FindObjectsOfType<CheckPoint>()
            .FirstOrDefault(cp => cp.isInitialCheckPoint);

        if (initial != null) {
            SetCheckPoint(initial.GetComponent<CheckPoint>());
            Debug.Log("�Զ�ע���ʼ�����㣺" + initial.name);
        }
    }

    private IEnumerator UnLoadPreviousScene(GameSceneDataSO gameSceneData) {
        // TODO:����
        //yield return new WaitForSeconds(fadeDuration);

        yield return currentGameSceneData.sceneReference.UnLoadScene();
        yield return LoadSence(gameSceneData);
        RegisterInitialCheckPoint(); // ע���ʼ�浵��

        // TODO:����
        yield return new WaitForSeconds(fadeDuration);
        GameManager.Instance.CreatePlayer(currentCheckPoint.spawnPoint.position);
    }
}
