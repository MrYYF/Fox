using System.Collections;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour {

    [Tooltip("ƽ̨���")] public Transform startPoint;
    [Tooltip("ƽ̨�յ�")] public Transform endPoint;
    [Tooltip("�ƶ��ٶ�")] public float dashSpeed = 2f;
    [Tooltip("�ƶ��ٶ�")] public float returnSpeed = 2f;
    [Tooltip("ͣ��ʱ��")] public float waitTime = 1f;

    private bool isMoving = false;
    private bool isPlayerOnPlatform = false; // ��¼����Ƿ���ƽ̨��
    private Vector2 currentVelocity; // ƽ̨��ǰ�ٶ�
    private Vector2 targetPosition; // ��ǰĿ��λ��

    private void Start() {
        startPoint.position = transform.position;
        targetPosition = endPoint.position;
        currentVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(transform); // ���������Ϊƽ̨���Ӷ���
            isPlayerOnPlatform = true; // ��������ƽ̨��

            if (!isMoving) {
                StartCoroutine(DashToEndPoint());
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
                currentVelocity.y = Mathf.Max(currentVelocity.y, 0); // ȷ��ƽ̨���ٶȲ�Ϊ��ֵ
                playerRb.velocity += currentVelocity; // ��ƽ̨���ٶȼӵ���ҵ��ٶ���
            }
        }
    }

    // �ƶ����յ�
    private IEnumerator DashToEndPoint() {
        isMoving = true;
        targetPosition = endPoint.position; // �л�Ŀ��λ��
        yield return new WaitForSeconds(waitTime); // �ȴ�һ��ʱ��
        currentVelocity = (endPoint.position - startPoint.position).normalized * dashSpeed;

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f) {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, dashSpeed * Time.fixedDeltaTime);
            transform.position = newPosition;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(ReturnToStartPoint());
        yield return new WaitForSeconds(0.1f);
        currentVelocity = Vector2.zero;
    }

    // �������
    private IEnumerator ReturnToStartPoint() {
        //TODO:����ʱ����ϴ������ƺ��ᵼ�����³�̵��յ�
        targetPosition = startPoint.position; // �л�Ŀ��λ��
        yield return new WaitForSeconds(waitTime); // �ȴ�һ��ʱ��
        currentVelocity = (startPoint.position - endPoint.position).normalized * returnSpeed;

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f) {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, returnSpeed * Time.fixedDeltaTime);
            transform.position = newPosition;
            yield return new WaitForFixedUpdate();
        }

        if (isPlayerOnPlatform) {
            StartCoroutine(DashToEndPoint());
        }
        yield return new WaitForSeconds(0.1f);
        currentVelocity = Vector2.zero;
        isMoving = false;
    }

    private void OnDrawGizmosSelected() {
        if (endPoint != null) {
            Gizmos.color = Color.green; // ����������ɫΪ��ɫ
            Gizmos.DrawLine(transform.position, endPoint.position); // ��������
        }
    }
}
