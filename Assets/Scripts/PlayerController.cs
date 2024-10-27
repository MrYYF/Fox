using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region 变量 Variables
    [Header("基础数值")]
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
    float groundDetectRadius = 0.35f;
    float crouchSpeed;

    bool isInvulnerable = false; // 是否处于无敌状态
    float invulnerabilityDuration = 1f; //无敌时间
    int currentHealth;
    #endregion

    //初始化组件
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        InitializeAbilityCount();
    }

    //初始化能力数值
    void InitializeAbilityCount()
    {
        currentSpeed = normalSpeed;
        jumpCount = PlayerManager.PlayerManagerInstance.maxJumpCount;
        dashCount = PlayerManager.PlayerManagerInstance.maxDashCount;
        currentHealth = PlayerManager.PlayerManagerInstance.maxHitPoint;
    }

    //重置能力次数
    void ResetAbilityCount()
    {
        jumpCount = PlayerManager.PlayerManagerInstance.maxJumpCount;
        dashCount = PlayerManager.PlayerManagerInstance.maxDashCount;
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

    #region 角色移动相关代码
    //实现平滑移动
    void Move()
    {
        Vector2 currentPosition = rigidbody2D.position; // 当前物体的位置
        Vector2 targetPosition = currentPosition + moveDirection * normalSpeed; //目标位置

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

    //TODO:土狼跳
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
        
        if (isCrouch) //下蹲时改变碰撞体
        {
            if (isGround)
                Mathf.SmoothDamp(currentSpeed, slideSpeed, ref currentSpeed, 0.5f);
            boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, -0.1f);
            boxCollider2D.size = new Vector2(boxCollider2D.size.x, 0.1f);
        } 
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, normalSpeed, ref crouchSpeed, 0.1f);
            boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, -0.06f);
            boxCollider2D.size = new Vector2(boxCollider2D.size.x, 0.2f);
        }
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
    #endregion

    #region 动画
    //TODO:之后需要将动画部分单独迁移到新的脚本中
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
    #endregion

    #region 受伤相关函数
    // 当玩家受到伤害时调用
    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            currentHealth = Mathf.Max(0, currentHealth - damage); // 防止血量小于0
            PlayerManager.PlayerManagerInstance.UpdateHealthUI(currentHealth);

            if (currentHealth == 0)
            {
                LevelManager.LevelManagerInstance.GameOver();
                return;
            }
            
            StartCoroutine(InjuredFlash());
        }
    }

    //受伤后闪烁
    public IEnumerator InjuredFlash()
    {
        Color originalColor = spriteRenderer.material.color; // 保存原始颜色
        Color flashColor = Color.red; // 闪烁时的颜色
        float flashInterval = 0.1f; // 闪烁间隔
        int flashCount = Mathf.FloorToInt(invulnerabilityDuration / flashInterval);

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.material.color = flashColor; // 设置为闪烁颜色
            yield return new WaitForSeconds(flashInterval/2);
            spriteRenderer.material.color = originalColor; // 恢复原始颜色
            yield return new WaitForSeconds(flashInterval/2);
        }

        // 恢复可受伤状态
        isInvulnerable = false;
    }

    //受伤后弹开
    public void InjuredBounceOff(Vector3 position)
    {
        Vector2 vector2 =  transform.position - position; //有害物相对角色的位置
        rigidbody2D.velocity = -rigidbody2D.velocity; //垂直方向反弹
        currentVelocity = vector2 * jumpForce; //水平方向反弹
    }
    #endregion
}
