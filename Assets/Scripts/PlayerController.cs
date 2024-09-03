using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("速度")]
    public float speed; //移动速度
    [Tooltip("跳跃推力")]
    public float jumpForce;
    [Tooltip("冲刺推力")]
    public float dashForce;

    public Transform groundDetectTransform;
    public LayerMask groundLayerMask;

    Vector2 currentVelocity;
    Vector2 moveDirection;

    //人物状态
    bool isJump;
    bool isJumpPressed;
    bool isGround;

    int jumpCount;
    int dashCount;

    float smoothTime = 1f;
    Rigidbody2D rigidbody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;
    
    

    //初始化组件
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetAbilityCount();
    }

    void Update()
    {
        
        MoveDirection();
        Move();
        Jump();
        AnimationController();
        //Dash();
    }

    //实现平滑移动
    void Move()
    {
        Vector2 currentPosition = transform.position; // 当前物体的位置
        Vector2 targetPosition = currentPosition + moveDirection * speed; //目标位置

        // 平滑移动到目标位置
        Vector2.SmoothDamp(
            currentPosition,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );

        rigidbody2D.velocity = new Vector2(currentVelocity.x, rigidbody2D.velocity.y);
    }

    void Jump()
    {
        isGround = Physics2D.OverlapCircle(groundDetectTransform.position, 0.1f, groundLayerMask);
        Debug.Log(isGround);
        
        if (Input.GetKeyDown(KeyCode.W) && jumpCount > 0)
        {
            isJumpPressed = true;
        }
            

        //在地面
        if (isGround)
        {
            isJump = false;
            ResetAbilityCount();
        } else if (!isJump)
        {
            isJump = true;
            jumpCount--;
        }

        if (isJumpPressed) //地面跳跃
        {
            isJump = true;
            isJumpPressed = false;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount--;
        }
    }

    //重置移动技能次数
    void ResetAbilityCount()
    {
        jumpCount = MainManager.Instance.maxJumpCount;
        dashCount = MainManager.Instance.maxDashCount;
    }

    //冲刺
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashCount > 0)
        {
            rigidbody2D.AddForce(moveDirection * dashForce, ForceMode2D.Impulse);
            dashCount--;
        }
    }

    //土狼跳
    //private IEnumerator CoyoteTime()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    if (jumpCount > 0)
    //    {
    //        jumpCount--;
    //        isJump = true;
    //    }

    //}

    //根据输入计算移动向量

    void MoveDirection()
    {
        if (Input.GetKey(KeyCode.A))
            moveDirection.x = -1;
        else if (Input.GetKey(KeyCode.D))
            moveDirection.x = 1;
        else if (Input.GetKey(KeyCode.W))
            moveDirection.y = 1;
        else if (Input.GetKey(KeyCode.S))
            moveDirection.y = -1;
        else
            moveDirection = Vector2.zero;
        moveDirection.Normalize();
    }

    //动画控制
    void AnimationController()
    {
        if (moveDirection.x != 0)
        {
            animator.SetBool("IsRunning", true);
            if (moveDirection.x < 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        } else
            animator.SetBool("IsRunning", false);


        if (rigidbody2D.velocity.y > 0)
            animator.SetBool("IsJumping", true);
        else if (rigidbody2D.velocity.y == 0 && moveDirection.y < 0)
            animator.SetBool("IsCrouching", true);
        else if (rigidbody2D.velocity.y < 0)
            animator.SetBool("IsFall", true);
        else if (rigidbody2D.velocity.y == 0)
        {
            animator.SetBool("IsFall", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsCrouching", false);
        }

        animator.SetFloat("ySpeed", rigidbody2D.velocity.y);
    }












}
