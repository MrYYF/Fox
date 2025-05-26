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
        GameManager.Instance.CreatePlayer(currentCheckPoint.spawnPoint.position);

        Debug.Log("Restarting level...");
    }

    // internal关键字
    internal void SetCheckPoint(CheckPoint checkPoint) {
        Debug.Log($"Setting checkpoint: {checkPoint.name} at position {checkPoint.spawnPoint.position}");
        if(currentCheckPoint != null && checkPoint != currentCheckPoint) {
            currentCheckPoint.isUsed = false; // 设置之前的存档点未使用
            checkPoint.isUsed = true; // 设置当前存档点为已使用
            currentCheckPoint = checkPoint;
        }
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
