using UnityEngine;

/// <summary>
/// 挂载到存档点，用于调用存档点的相关方法
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class CheckPoint : MonoBehaviour {
    [Tooltip("重生位置")] public Transform spawnPoint;
    [Tooltip("检查点是否已使用")] public bool isUsed;
    [Tooltip("是否初始检查点")] public bool isInitialCheckPoint;
    [Tooltip("是否传送点")] public bool isPortal;
    public GameSceneDataSO gameSceneData; // 关联的场景数据
    public SceneLoadEventSO sceneLoadEvent;

    private Collider2D checkPointArea; // 存档点触发器区域

    private void Awake() {
        checkPointArea = GetComponent<Collider2D>();
        checkPointArea.isTrigger = true;// 确保触发器区域是触发器
    }

    private void Start() {
        if (isInitialCheckPoint) {
            SenceSaveManager.Instance.SetCheckPoint(this); // 如果是初始检查点，设置为当前存档点
        }
    }


    //SenceSaveManager，将当前存档点设置为当前存档点
    public void SetThisCheckPoint() {
        SenceSaveManager.Instance.SetCheckPoint(this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isUsed && isPortal) {
            isUsed = true; // 设置为已使用
            sceneLoadEvent.RaiseEvent(gameSceneData); // 如果是传送点，触发场景加载事件
        }
        else if (collision.CompareTag("Player") && !isUsed) {
            SetThisCheckPoint();
            //TODO: UI显示
            //TODO: 粒子效果播放
            //TODO: 音效播放
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            //TODO: UI隐藏
        }
    }

}
