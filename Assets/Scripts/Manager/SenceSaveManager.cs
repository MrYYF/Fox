using System;
using System.Collections;
using System.Linq;
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
    [Tooltip("�����л��¼�")] public SceneLoadEventSO senceLoadEvent;

    [Header("��������")]
    [Tooltip("��һ����������")]public GameSceneDataSO firstGameSceneData;
    [Tooltip("��ǰ��������")]public GameSceneDataSO currentGameSceneData;
    [Tooltip("��һ�����س�������")]public GameSceneDataSO nextGameSceneData;
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

    private IEnumerator UnLoadPreviousScene() {
        // TODO:���뽥��

        yield return new WaitForSeconds(fadeDuration);
        Debug.Log("����ж�س�������currentGameSceneData${}", currentGameSceneData);
        if (currentGameSceneData != null)
            yield return currentGameSceneData.sceneReference.UnLoadScene();

        LoadSence(nextGameSceneData);
        //CheckPoint initialCheckPoint = FindObjectsOfType<CheckPoint>()
        //    .FirstOrDefault(cp => cp.isInitialCheckPoint);
        //if (initialCheckPoint != null) {
        //    SetCheckPoint(initialCheckPoint); // ���õ�ǰ�浵��
        //}
    }
}
