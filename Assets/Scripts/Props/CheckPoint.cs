using UnityEngine;

/**
 * 挂载到存档点，用于调用存档点的相关方法
 */
public class CheckPoint : MonoBehaviour
{
    [Tooltip("重生位置")]public Transform spawnPoint;
    [Tooltip("触发区域")]public Collider2D checkPointArea;
    [Tooltip("检查点是否已使用")]public bool isUsed;

    private bool isStayInCheckPointArea; //是否处于触发区域


    //调用SaveManager，将当前存档点设置为当前存档点
    public void SetCheckPoint() {
        if (isStayInCheckPointArea) {
            SaveManager.Instance.SetCheckPoint(this);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player") && !isUsed) {
            isStayInCheckPointArea = true;
            SetCheckPoint();
            //TODO: UI显示
            //TODO: 粒子效果播放
            //TODO: 音效播放
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            isStayInCheckPointArea = false;
            //TODO: UI隐藏
        }
    }

}
