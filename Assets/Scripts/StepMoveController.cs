using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[RequireComponent(typeof(Rigidbody2D))]
public class StepMoveController : MonoBehaviour
{
    public Vector2 end;
    public float speed =2f;
    
    Vector2 start;
    Vector2 target; // 当前目标点
    bool direction = true;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = end;
        start = transform.position;
    }

    void Update()
    {
        // 计算物体当前位置到目标点的距离
        float step = speed * Time.fixedDeltaTime; // 每帧移动的距离
        Vector2 newPosition = Vector2.MoveTowards(rb.position, target, step);
        //Vector2 newPosition = Vector2.SmoothDamp(rb.position, target, ref velocity, smoothTime, speed, Time.fixedDeltaTime);
        rb.MovePosition(newPosition);



        // 检查物体是否到达目标点
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            // 切换目标点
            direction = !direction;
            target = direction ? end : start;
        }
    }


}
