using System.Collections;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour {

    [Tooltip("平台起点")] public Transform startPoint;
    [Tooltip("平台终点")] public Transform endPoint;
    [Tooltip("移动速度")] public float dashSpeed = 2f;
    [Tooltip("移动速度")] public float returnSpeed = 2f;
    [Tooltip("停留时间")] public float waitTime = 1f;

    private bool isMoving = false;
    private bool isPlayerOnPlatform = false; // 记录玩家是否在平台上
    private Vector2 currentVelocity; // 平台当前速度
    private Vector2 targetPosition; // 当前目标位置

    private void Start() {
        startPoint.position = transform.position;
        targetPosition = endPoint.position;
        currentVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(transform); // 将玩家设置为平台的子对象
            isPlayerOnPlatform = true; // 标记玩家在平台上

            if (!isMoving) {
                StartCoroutine(DashToEndPoint());
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(null); // 解除玩家与平台的子对象关系
            isPlayerOnPlatform = false; // 标记玩家离开平台

            // 将平台的动量传递给玩家
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null) {
                currentVelocity.y = Mathf.Max(currentVelocity.y, 0); // 确保平台的速度不为负值
                playerRb.velocity += currentVelocity; // 将平台的速度加到玩家的速度上
            }
        }
    }

    // 移动到终点
    private IEnumerator DashToEndPoint() {
        isMoving = true;
        targetPosition = endPoint.position; // 切换目标位置
        yield return new WaitForSeconds(waitTime); // 等待一段时间
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

    // 返回起点
    private IEnumerator ReturnToStartPoint() {
        //TODO:返回时如果断触或触碰似乎会导致重新冲刺到终点
        targetPosition = startPoint.position; // 切换目标位置
        yield return new WaitForSeconds(waitTime); // 等待一段时间
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
            Gizmos.color = Color.green; // 设置连线颜色为绿色
            Gizmos.DrawLine(transform.position, endPoint.position); // 绘制连线
        }
    }
}
