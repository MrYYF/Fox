using Cinemachine;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager> {
    public GameObject playerPrefab; // 玩家预制体
    private GameObject player; // 玩家实例
    private CinemachineVirtualCamera virtualCamera;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    //注册玩家到游戏管理器
    public void RigisterPlayer(GameObject rigister) {
        if (rigister == null) return;
        player = rigister;
        BindCamera();
        //其它初始化操作
    }

    //创建玩家实例
    public void CreatePlayer(Vector3 spawnPoint) {
        if (player == null) {
            player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        }
        else {
            Debug.LogError("Player already exists. Use DestroyPlayer() to remove the existing player before creating a new one.");
        }
    }

    //销毁玩家实例
    public void DestroyPlayer() {
        if (player != null) {
            Destroy(player);
            player = null;
        }
        else {
            Debug.LogError("Player not found to destroy.");
        }
    }

    //获取玩家实例
    public GameObject GetPlayer() {
        if (player != null) return player;

        //如果没有玩家实例，则查找场景中的玩家对象
        player = GameObject.FindWithTag("Player");
        if (player != null) {
            return player;
        }
        else {
            Debug.LogError("Player not found in the scene.");
            return null;
        }
    }

    //绑定玩家和摄像机
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
