using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class SaveManager : Singleton<SaveManager>
{
    PlayerInputControl playerInputControl; //输入系统
    GameObject playerPrefab; //玩家预制体
    CheckPoint currentCheckPoint; //当前存档点
    Transform spawnPoint; //重生点

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        playerInputControl = new PlayerInputControl();
        playerInputControl.Gameplay.Restart.started += Restart;
    }
    void OnEnable() {
        playerInputControl?.Enable();
    }
    void OnDisable() {
        playerInputControl?.Disable();
    }

    // 重新加载当前场景
    private void Restart(InputAction.CallbackContext context) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Restarting level...");
    }

    // internal关键字
    internal void SetCheckPoint(Vector3 position) {
        spawnPoint.position = position;
    }
}
