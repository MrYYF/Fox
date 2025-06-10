using UnityEngine;

/**
 * 挂载到存档点，用于调用存档点的相关方法
 */
[RequireComponent(typeof(BoxCollider2D))]
public class CheckPoint : MonoBehaviour {
    [Tooltip("重生位置")] public Transform spawnPoint;
    [Tooltip("检查点是否已使用")] public bool isUsed;
    [Tooltip("是否初始检查点")] public bool isInitialCheckPoint;

    private Collider2D checkPointArea; // 存档点触发器区域

    private void Awake() {
        checkPointArea = checkPointArea.GetComponent<Collider2D>();
        checkPointArea.isTrigger = true;// 确保触发器区域是触发器
    }

    private void Start() {
        if (isInitialCheckPoint) {
            SaveManager.Instance.SetCheckPoint(this); // 如果是初始检查点，设置为当前存档点
        }
    }


    //调用SaveManager，将当前存档点设置为当前存档点
    public void SetThisCheckPoint() {
        SaveManager.Instance.SetCheckPoint(this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isUsed) {
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
