using UnityEngine;

/**
 * 用于实现移动平台功能
 */
[RequireComponent(typeof(Rigidbody2D))]
public class StepMoveController : MonoBehaviour
{
    public Vector2 end;
    public float speed =2f;


    Vector2 start;
    Vector2 target; // 当前目标点
    bool direction = true;
    Rigidbody2D rigidbody2D;
    Rigidbody2D playerRigidbody2D;

    float step; // 每帧移动的距离
    Vector2 newPosition;
    Vector2 platformMovement;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        target = end;
        start = transform.position;
    }

    
    void FixedUpdate()
    {
        // 计算物体当前位置到目标点的距离
        step = speed * Time.fixedDeltaTime; // 每帧移动的距离
        newPosition = Vector2.MoveTowards(rigidbody2D.position, target, step);
        platformMovement = newPosition - rigidbody2D.position;
        rigidbody2D.MovePosition(newPosition);
        if (playerRigidbody2D != null)
        {
            //playerRigidbody2D.MovePosition(playerRigidbody2D.position+platformMovement);
            playerRigidbody2D.position += platformMovement;
        }

        // 检查物体是否到达目标点
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            // 切换目标点
            direction = !direction;
            target = direction ? end : start;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                playerRigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        playerRigidbody2D = null;
    }
}
