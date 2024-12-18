using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region 变量 Variables
    [Header("基础数值")]
    [Tooltip("速度基准值")] public float speed;
    float currentSpeed; //当前速度
    [Tooltip("移动平滑时间")] public float smoothTime;
    [Tooltip("滑铲速度修正")] public float slideSpeedModify; //TODO:滑铲后续用超过一定速度后下蹲会施加推力同时减少速度上限的方法来实现
    [Tooltip("滑铲推力")] public float slideForce;
    [Tooltip("跳跃推力")] public float jumpForce;
    [Tooltip("冲刺推力")] public float dashForce;

    PlayerInputControl playerInputControl; //输入系统
    Vector2 inputDirection; //输入方向
    PhysicsCheck physicsCheck; //物理检测类
    Rigidbody2D rb;
    CapsuleCollider2D col;

    //人物状态
    [HideInInspector]public bool isCrouch;
    bool isInvulnerable = false; // 是否处于无敌状态
    float invulnerabilityDuration = 1f; //无敌时间
    int currentHealth;
    #endregion

    #region 生命周期函数
    //初始化组件
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        col = GetComponent<CapsuleCollider2D>();
        playerInputControl = new PlayerInputControl();

        playerInputControl.Gameplay.Jump.started += Jump;
        playerInputControl.Gameplay.Dash.started += Dash;
    }
    void OnEnable()
    {
        playerInputControl?.Enable();
    }
    void OnDisable()
    {
        playerInputControl?.Disable();
    }
    void Update()
    {
        inputDirection = playerInputControl.Gameplay.Move.ReadValue<Vector2>(); //获取输入移动向量
        Crouch();
    }
    void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region 角色移动相关代码
    //实现平滑移动
    void Move()
    {
        //平滑移动
        Vector2 targetPosition = rb.position + inputDirection * currentSpeed; //目标位置
        Vector2 currentVelocity = rb.velocity;
        Vector2.SmoothDamp(rb.position, targetPosition, ref currentVelocity, smoothTime);
        rb.velocity = new Vector2(currentVelocity.x, rb.velocity.y);
        //调整朝向
        if(currentVelocity.x < 0) transform.localScale = new Vector3( -1, 1, 1);
        if(currentVelocity.x > 0) transform.localScale = new Vector3( 1, 1, 1);
    }
    //跳跃
    void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        ////在地面
        //if (isGround)
        //{
        //    isJump = false;
        //    //ResetAbilityCount();
        //} else if (!isJump)
        //{
        //    isJump = true;
        //    jumpCount--;
        //}

        //if (isJumpPressed) //地面跳跃
        //{
        //    isJump = true;
        //    isJumpPressed = false;
        //    rb.velocity = new Vector2(rb.velocity.x, 0);
        //    if (isCrouch && isGround)
        //        rb.AddForce(Vector2.up * jumpForce * 1.5f, ForceMode2D.Impulse); //下蹲跳
        //    else
        //    jumpCount--;
        //}
    }
    //冲刺
    void Dash(InputAction.CallbackContext context)
    {
        rb.AddForce(inputDirection * dashForce, ForceMode2D.Impulse);
    }
    //TODO:蹲伏
    void Crouch()
    {
        if(inputDirection.y < 0) //下蹲
        {
            isCrouch = true;
            currentSpeed = speed * slideSpeedModify;
            col.offset = new Vector2(col.offset.x, -0.5f);
            col.size = new Vector2(col.size.x, 1);
        }
        else if (isCrouch && !physicsCheck.isRoof) //站起
        {
            isCrouch = false;
            currentSpeed = speed;
            col.offset = new Vector2(col.offset.x, -0.3f);
            col.size = new Vector2(col.size.x, 1.4f);
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

    #endregion

    #region 动画
    //TODO:之后需要将动画部分单独迁移到新的脚本中
    //动画控制
    //void AnimationController()
    //{
    //    if (Mathf.Abs(rigidbody2D.velocity.x) > 1f)
    //    {
    //        animator.SetBool("IsRunning", true);
    //        if (inputDirection.x < 0)
    //        else if (inputDirection.x > 0)
    //    } else
    //        animator.SetBool("IsRunning", false);

    //    if (isJump && rigidbody2D.velocity.y < 0)
    //        animator.SetBool("IsFall", true);
    //    else
    //        animator.SetBool("IsFall", false);

    //    animator.SetBool("IsJumping", isJump);
    //    animator.SetBool("IsCrouching", isCrouch);
    //}
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
            
        }
    }

    

    //受伤后弹开
    public void InjuredBounceOff(Vector3 position)
    {
        Vector2 vector2 =  transform.position - position; //有害物相对角色的位置
        rb.velocity = -rb.velocity; //垂直方向反弹
        //currentVelocity = vector2 * jumpForce; //水平方向反弹
    }
    #endregion

    //初始化能力数值
    //void InitializeAbilityCount()
    //{
    //    currentSpeed = normalSpeed;
    //    jumpCount = PlayerManager.PlayerManagerInstance.maxJumpCount;
    //    dashCount = PlayerManager.PlayerManagerInstance.maxDashCount;
    //    currentHealth = PlayerManager.PlayerManagerInstance.maxHitPoint;
    //}

    //重置能力次数
    //void ResetAbilityCount()
    //{
    //    jumpCount = PlayerManager.PlayerManagerInstance.maxJumpCount;
    //    dashCount = PlayerManager.PlayerManagerInstance.maxDashCount;
    //}
}
