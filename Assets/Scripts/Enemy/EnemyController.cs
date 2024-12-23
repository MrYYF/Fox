using UnityEngine;

/// <summary>
/// 敌人控制脚本
/// </summary>
public class EnemyController : MonoBehaviour
{
    //TODO:状态机
    //TODO:不同攻击模式、巡逻模式、追逐
    [Header("基本配置")]
    [Tooltip("速度基准值")]public float normalSpeed;
    float currentSpeed;
    [Tooltip("追逐速度")]public float chaseSpeed;
    [Tooltip("等待时间")]public float stayTime;
    float stayTimeCounter;
    bool isStay;

    Rigidbody2D rb;
    Vector2 faceDirection;
    PhysicsCheck physicsCheck;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        stayTimeCounter = stayTime;
        isStay = false;
        currentSpeed = normalSpeed;
        faceDirection = new Vector2(transform.localScale.x, 0);
    }
    void Update()
    {
        if (isStay)
        {
            stayTimeCounter -= Time.deltaTime;
            if (stayTimeCounter < 0)
            {
                isStay = false;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }
        }
        else if (physicsCheck.isWall && !isStay)
        {
            stayTimeCounter = stayTime;
            isStay = true;
        }

    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (isStay)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        //平滑移动
        faceDirection = new Vector2(transform.localScale.x, 0);
        Vector2 targetPosition = rb.position + faceDirection * currentSpeed; //目标位置
        Vector2 currentVelocity = rb.velocity;
        Vector2.SmoothDamp(rb.position, targetPosition, ref currentVelocity, 1);
        rb.velocity = new Vector2(currentVelocity.x, rb.velocity.y);
    }

    //进入伤害判定范围受到伤害
    void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(GetComponent<Character>());
    }

    
}
