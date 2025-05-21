using Cinemachine;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager> {
    public GameObject playerPrefab; // ���Ԥ����
    private GameObject player; // ���ʵ��
    private CinemachineVirtualCamera virtualCamera;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    //ע����ҵ���Ϸ������
    public void RigisterPlayer(GameObject rigister) {
        if (rigister == null) return;
        player = rigister;
        BindCamera();
        //������ʼ������
    }

    //�������ʵ��
    public void CreatePlayer(Vector3 spawnPoint) {
        if (player == null) {
            player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        }
        else {
            Debug.LogError("Player already exists. Use DestroyPlayer() to remove the existing player before creating a new one.");
        }
    }

    //�������ʵ��
    public void DestroyPlayer() {
        if (player != null) {
            Destroy(player);
            player = null;
        }
        else {
            Debug.LogError("Player not found to destroy.");
        }
    }

    //��ȡ���ʵ��
    public GameObject GetPlayer() {
        if (player != null) return player;

        //���û�����ʵ��������ҳ����е���Ҷ���
        player = GameObject.FindWithTag("Player");
        if (player != null) {
            return player;
        }
        else {
            Debug.LogError("Player not found in the scene.");
            return null;
        }
    }

    //����Һ������
    public void BindCamera() {
        if (GetPlayer() == null) return;
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera != null) {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
        else {
            Debug.LogError("CinemachineVirtualCamera not found in the scene.");
        }
    }
}
