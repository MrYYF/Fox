using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GemController : MonoBehaviour
{
    [Tooltip("上下浮动的幅度")]
    public float floatAmplitude = 0.5f;
    [Tooltip("浮动的速度")]
    public float floatSpeed = 4f;

    
    private Vector3 startPos;
    private Animator animator;
    private bool isPickedUp = false; // 防止多次拾取

    private void Start()
    {
        animator = GetComponent<Animator>();

        // 记录初始位置
        startPos = transform.position;
    }

    void Update()
    {
        // 计算新的垂直位置
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // 应用新的位置
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !isPickedUp)
        {
            isPickedUp = true;

            // 触发Feedback动画
            animator.SetTrigger("PickedUp");

            // 禁用碰撞体，防止重复触发
            GetComponent<Collider2D>().enabled = false;
        }
    }

    // 这个方法将在动画结束时通过动画事件调用
    public void OnFeedbackAnimationEnd()
    {
        // 销毁游戏对象
        Destroy(gameObject);
    }
}
