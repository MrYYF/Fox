using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("普通速度")]
    public float normalSpeed;
    [Tooltip("滑铲速度")]
    public float slideSpeed;
    [Tooltip("跳跃推力")]
    public float jumpForce;
    [Tooltip("冲刺推力")]
    public float dashForce;

    public Transform groundDetectTransform;
    public Transform topDetectTransform;
    public LayerMask groundLayerMask;

    Vector2 currentVelocity;
    Vector2 moveDirection;

    //人物状态
    bool isJump;
    bool isJumpPressed;
    bool isGround;

    bool isCrouch;
    bool isCrouchPressed;
    bool canStandUp;

    int jumpCount;
    int dashCount;

    Rigidbody2D rigidbody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;

    float currentSpeed; //移动速度
    float groundDetectRadius = 0.37f;
    float temp;


    //初始化组件
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        ResetAbilityCount();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        MoveDirection();
        Move();
        Jump();
        Crouch();
        AnimationController();
        //Dash();
    }

    //实现平滑移动
    void Move()
    {
        Vector2 currentPosition = rigidbody2D.position; // 当前物体的位置
        Vector2 targetPosition = currentPosition + moveDirection * currentSpeed; //目标位置

        // 平滑移动到目标位置
        Vector2.SmoothDamp(
            currentPosition,
            targetPosition,
            ref currentVelocity,
            1f
        );

        rigidbody2D.velocity = new Vector2(currentVelocity.x, rigidbody2D.velocity.y);
        
    }

    void Jump()
    {
        isGround = Physics2D.OverlapCircle(groundDetectTransform.position, groundDetectRadius, groundLayerMask);
        
        if ( (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && jumpCount > 0)
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
            if (isCrouch && isGround)
                rigidbody2D.AddForce(Vector2.up * jumpForce * 1.5f, ForceMode2D.Impulse); //下蹲跳
            else
                rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpCount--;
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

    void Crouch()
    {
        canStandUp = !Physics2D.OverlapCircle(topDetectTransform.position, groundDetectRadius, groundLayerMask);

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            isCrouchPressed = true;
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            isCrouchPressed = false;

        if (isCrouchPressed)
            isCrouch = true;
        else if (!isCrouchPressed && canStandUp)
            isCrouch = false;
        else if (!canStandUp && !isJump)
            isCrouch = true;

        if (isCrouch)
        {
            if (isGround)
                Mathf.SmoothDamp(currentSpeed, slideSpeed, ref currentSpeed, 0.5f);
            boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, -0.1f);
            boxCollider2D.size = new Vector2(boxCollider2D.size.x, 0.1f);
        } 
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, normalSpeed, ref temp, 0.1f);
            boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, -0.06f);
            boxCollider2D.size = new Vector2(boxCollider2D.size.x, 0.2f);
        }
    }

    //重置移动技能次数
    void ResetAbilityCount()
    {
        jumpCount = PlayerManager.PlayerManagerInstance.maxJumpCount;
        dashCount = PlayerManager.PlayerManagerInstance.maxDashCount;
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

    //根据输入计算移动向量
    void MoveDirection()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveDirection.x = -1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveDirection.x = 1;
        else
            moveDirection = Vector2.zero;
        moveDirection.Normalize();
    }

    //动画控制
    void AnimationController()
    {
        if (Mathf.Abs(rigidbody2D.velocity.x) > 1f)
        {
            animator.SetBool("IsRunning", true);
            if (moveDirection.x < 0)
                spriteRenderer.flipX = true;
            else if (moveDirection.x > 0)
                spriteRenderer.flipX = false;
        } else
            animator.SetBool("IsRunning", false);

        if (isJump && rigidbody2D.velocity.y < 0)
            animator.SetBool("IsFall", true);
        else
            animator.SetBool("IsFall", false);

        animator.SetBool("IsJumping", isJump);
        animator.SetBool("IsCrouching", isCrouch);
    }

    //受伤后闪烁
    public IEnumerator FlashCoroutine()
    {
        Color originalColor = spriteRenderer.material.color; // 保存原始颜色
        Color flashColor = Color.red; // 闪烁时的颜色
        float flashInterval = 0.1f; // 闪烁间隔
        int flashCount = Mathf.FloorToInt(PlayerManager.PlayerManagerInstance.invulnerabilityDuration / flashInterval);

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.material.color = flashColor; // 设置为闪烁颜色
            yield return new WaitForSeconds(flashInterval/2);
            spriteRenderer.material.color = originalColor; // 恢复原始颜色
            yield return new WaitForSeconds(flashInterval/2);
        }

        // 恢复可受伤状态
        PlayerManager.PlayerManagerInstance.isInvulnerable = false;
    }

}
