using System.Collections;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour {

    [Tooltip("平台起点")] public Transform startPoint;
    [Tooltip("平台终点")] public Transform endPoint;
    [Tooltip("移动时间")] public float moveTime = 2f;
    [Tooltip("停留时间")] public float waitTime = 1f;

    private bool isMoving = false;
    private bool isPlayerOnPlatform = false; // 记录玩家是否在平台上
    private Vector2 currentVelocity = Vector2.zero; // 平台当前速度
    private Vector2 targetPosition; // 当前目标位置

    private void Start() {
        startPoint.position = transform.position;
        targetPosition = endPoint.position; // 初始目标为终点
    }

    private void FixedUpdate() {
        if (isMoving) {
            // 使用 SmoothDamp 平滑移动平台
            Vector2 newPosition = Vector2.SmoothDamp(transform.position, targetPosition, ref currentVelocity, moveTime);
            Debug.Log(currentVelocity);
            transform.position = newPosition;

            // 检查是否到达目标点
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f) {
                if (targetPosition == (Vector2)endPoint.position) {
                    // 到达终点，切换到起点
                    StartCoroutine(WaitAndSwitchTarget(startPoint.position));
                }
                else {
                    // 到达起点，切换到终点
                    StartCoroutine(WaitAndSwitchTarget(endPoint.position));
                }
            }
        }
    }

    private IEnumerator WaitAndSwitchTarget(Vector2 newTarget) {
        isMoving = false; // 暂停移动
        yield return new WaitForSeconds(waitTime); // 等待一段时间
        targetPosition = newTarget; // 切换目标位置
        isMoving = true; // 继续移动
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(transform); // 将玩家设置为平台的子对象
            isPlayerOnPlatform = true; // 标记玩家在平台上

            if (!isMoving) {
                isMoving = true; // 启动平台移动
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
                playerRb.velocity += currentVelocity * 2f; // 将平台的速度加到玩家的速度上
            }
        }
    }
}
