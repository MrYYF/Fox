using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelManager : Singleton<LevelManager>
{
    PlayerInputControl playerInputControl; //输入系统

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

    private void Restart(InputAction.CallbackContext context) {
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Restarting level...");
    }
}
