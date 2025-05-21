using UnityEngine;

/**
 * ���ص��浵�㣬���ڵ��ô浵�����ط���
 */
public class CheckPoint : MonoBehaviour
{
    [Tooltip("����λ��")]public Transform spawnPoint;
    [Tooltip("��������")]public Collider2D checkPointArea;

    private bool isStayInCheckPointArea; //�Ƿ��ڴ�������
    private bool isUsed; //�����Ƿ���ʹ��


    //����SaveManager������ǰ�浵������Ϊ��ǰ�浵��
    public void SetCheckPoint() {
        if (isStayInCheckPointArea) {
            SaveManager.Instance.SetCheckPoint(this);
            Debug.Log("�浵��������Ϊ: " + spawnPoint.position);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            isStayInCheckPointArea = true;
            SetCheckPoint();
            //TODO: UI��ʾ
            //TODO: ����Ч������
            //TODO: ��Ч����
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            isStayInCheckPointArea = false;
            //TODO: UI����
        }
    }

}
