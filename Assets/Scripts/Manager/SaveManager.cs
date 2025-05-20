using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class SaveManager : Singleton<SaveManager>
{
    PlayerInputControl playerInputControl; //����ϵͳ
    GameObject playerPrefab; //���Ԥ����
    CheckPoint currentCheckPoint; //��ǰ�浵��
    Transform spawnPoint; //������

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

    // ���¼��ص�ǰ����
    private void Restart(InputAction.CallbackContext context) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Restarting level...");
    }

    // internal�ؼ���
    internal void SetCheckPoint(Vector3 position) {
        spawnPoint.position = position;
    }
}
