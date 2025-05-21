using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class SaveManager : Singleton<SaveManager>
{
    // TODO: 用SO存储数据
    PlayerInputControl playerInputControl; //输入系统
    public CheckPoint currentCheckPoint; //当前存档点
    public Transform spawnPoint; //重生点

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
        // 删除当前玩家
        GameManager.Instance.DestroyPlayer();
        // 在当前存档点重生
        GameManager.Instance.CreatePlayer(spawnPoint.position);

        Debug.Log("Restarting level...");
    }

    // internal关键字
    internal void SetCheckPoint(CheckPoint checkPoint) {
        currentCheckPoint = checkPoint;
        spawnPoint.position = checkPoint.spawnPoint.position;
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
