using UnityEngine;

/**
 * ���ص��浵�㣬���ڵ��ô浵�����ط���
 */
[RequireComponent(typeof(BoxCollider2D))]
public class CheckPoint : MonoBehaviour {
    [Tooltip("����λ��")] public Transform spawnPoint;
    [Tooltip("�����Ƿ���ʹ��")] public bool isUsed;
    [Tooltip("�Ƿ��ʼ����")] public bool isInitialCheckPoint;

    private Collider2D checkPointArea; // �浵�㴥��������

    private void Awake() {
        checkPointArea = checkPointArea.GetComponent<Collider2D>();
        checkPointArea.isTrigger = true;// ȷ�������������Ǵ�����
    }

    private void Start() {
        if (isInitialCheckPoint) {
            SaveManager.Instance.SetCheckPoint(this); // ����ǳ�ʼ���㣬����Ϊ��ǰ�浵��
        }
    }


    //����SaveManager������ǰ�浵������Ϊ��ǰ�浵��
    public void SetThisCheckPoint() {
        SaveManager.Instance.SetCheckPoint(this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isUsed) {
            SetThisCheckPoint();
            //TODO: UI��ʾ
            //TODO: ����Ч������
            //TODO: ��Ч����
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            //TODO: UI����
        }
    }

}
