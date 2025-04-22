using System.Collections;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour {

    [Tooltip("ƽ̨���")] public Transform startPoint;
    [Tooltip("ƽ̨�յ�")] public Transform endPoint;
    [Tooltip("�ƶ�ʱ��")] public float moveTime = 2f;
    [Tooltip("ͣ��ʱ��")] public float waitTime = 1f;

    private bool isMoving = false;
    private bool isPlayerOnPlatform = false; // ��¼����Ƿ���ƽ̨��
    private Vector2 currentVelocity = Vector2.zero; // ƽ̨��ǰ�ٶ�
    private Vector2 targetPosition; // ��ǰĿ��λ��

    private void Start() {
        startPoint.position = transform.position;
        targetPosition = endPoint.position; // ��ʼĿ��Ϊ�յ�
    }

    private void FixedUpdate() {
        if (isMoving) {
            // ʹ�� SmoothDamp ƽ���ƶ�ƽ̨
            Vector2 newPosition = Vector2.SmoothDamp(transform.position, targetPosition, ref currentVelocity, moveTime);
            Debug.Log(currentVelocity);
            transform.position = newPosition;

            // ����Ƿ񵽴�Ŀ���
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f) {
                if (targetPosition == (Vector2)endPoint.position) {
                    // �����յ㣬�л������
                    StartCoroutine(WaitAndSwitchTarget(startPoint.position));
                }
                else {
                    // ������㣬�л����յ�
                    StartCoroutine(WaitAndSwitchTarget(endPoint.position));
                }
            }
        }
    }

    private IEnumerator WaitAndSwitchTarget(Vector2 newTarget) {
        isMoving = false; // ��ͣ�ƶ�
        yield return new WaitForSeconds(waitTime); // �ȴ�һ��ʱ��
        targetPosition = newTarget; // �л�Ŀ��λ��
        isMoving = true; // �����ƶ�
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(transform); // ���������Ϊƽ̨���Ӷ���
            isPlayerOnPlatform = true; // ��������ƽ̨��

            if (!isMoving) {
                isMoving = true; // ����ƽ̨�ƶ�
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(null); // ��������ƽ̨���Ӷ����ϵ
            isPlayerOnPlatform = false; // �������뿪ƽ̨

            // ��ƽ̨�Ķ������ݸ����
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null) {
                playerRb.velocity += currentVelocity * 2f; // ��ƽ̨���ٶȼӵ���ҵ��ٶ���
            }
        }
    }
}
