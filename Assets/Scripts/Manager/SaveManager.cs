using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class SaveManager : Singleton<SaveManager>
{
    // TODO: ��SO�洢����
    PlayerInputControl playerInputControl; //����ϵͳ
    public CheckPoint currentCheckPoint; //��ǰ�浵��
    public Transform spawnPoint; //������

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
        // ɾ����ǰ���
        GameManager.Instance.DestroyPlayer();
        // �ڵ�ǰ�浵������
        GameManager.Instance.CreatePlayer(currentCheckPoint.spawnPoint.position);

        Debug.Log("Restarting level...");
    }

    // internal�ؼ���
    internal void SetCheckPoint(CheckPoint checkPoint) {
        Debug.Log($"Setting checkpoint: {checkPoint.name} at position {checkPoint.spawnPoint.position}");
        if(currentCheckPoint != null && checkPoint != currentCheckPoint) {
            currentCheckPoint.isUsed = false; // ����֮ǰ�Ĵ浵��δʹ��
            checkPoint.isUsed = true; // ���õ�ǰ�浵��Ϊ��ʹ��
            currentCheckPoint = checkPoint;
        }
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
