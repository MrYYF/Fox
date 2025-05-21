using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家控制类
/// </summary>
public class PlayerController : MonoBehaviour {
    #region 
    /**
     * 变量 Variables
     */
    [Header("基础数值")]
    [Tooltip("速度基准值")] public float normalSpeed = 10f;
    float currentSpeed; //当前速度
    [Tooltip("移动平滑时间")] public float smoothTime = 0.8f;
    [Tooltip("滑铲速度修正")] public float slideSpeed = 8f;
    [Tooltip("攀爬速度修正")] public float climbSpeed = 3f;
    [Tooltip("滑落速度")]public float slideDownSpeed = 2f;
    [Tooltip("攀爬体力")] public float climbStamina = 5f;
    float currentStamina;
    float gravityScale = 4f; // 重力
    [Tooltip("滑铲推力")] public float slideForce = 10f;
    [Tooltip("跳跃推力")] public float jumpForce = 10f;
    [Tooltip("持续跳跃推力")] public float holdJumpForce = 40f;
    [Tooltip("持续跳跃推力时间")] public float holdJumpTime = 0.15f;
    float holdJumpCounter; // 持续跳跃计时器
    [Tooltip("土狼跳时间")] public float coyoteTime = 0.1f;
    float coyoteTimeCounter; //土狼跳计时器
    [Tooltip("跳跃预输入时间")] public float jumpBufferTime = 0.1f; // 跳跃预输入时间
    float jumpBufferCounter; // 跳跃预输入计时器
    [Tooltip("冲刺推力")] public float dashForce = 10f;
    [Tooltip("冲刺时间")] public float dashTime = 0.2f; // 冲刺时间


    /**
     * 组件
     */
    PlayerInputControl playerInputControl; //输入系统
    Vector2 inputDirection; //输入方向
    PhysicsCheck physicsCheck; //物理检测类
    Rigidbody2D rb;
    AudioDefination audioDefination; //音效类
    //CapsuleCollider2D col;

    /**
     * 状态
     */
    [HideInInspector] public bool isJump;
    [HideInInspector] public bool isClimb;
    [HideInInspector] public bool isDash;
    [HideInInspector] public bool isCrouch;
    [HideInInspector] public bool isHurt;

    #endregion

    #region 生命周期函数
    //初始化组件
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        audioDefination = GetComponent<AudioDefination>();
        playerInputControl = new PlayerInputControl();
        playerInputControl.Gameplay.Jump.started += StartJump;
        playerInputControl.Gameplay.Jump.canceled += CancelJump;
        playerInputControl.Gameplay.Climb.started += StartClimb;
        playerInputControl.Gameplay.Climb.canceled += CancelClimb;
        playerInputControl.Gameplay.Dash.started += Dash;
    }
    void OnEnable() {
        playerInputControl?.Enable();
    }
    void OnDisable() {
        playerInputControl?.Disable();
    }
    void Start() {
        GameManager.Instance.RigisterPlayer(gameObject); //注册玩家到游戏管理器
        currentSpeed = normalSpeed;
        coyoteTimeCounter = coyoteTime;
        gravityScale = rb.gravityScale;
        jumpBufferCounter = 0;
        holdJumpCounter = 0;
    }
    void Update() {
        if (isHurt) return;
        inputDirection = playerInputControl.Gameplay.Move.ReadValue<Vector2>(); //获取输入移动向量
        // 土狼跳逻辑
        CoyoteJump();
        // 跳跃预输入逻辑
        JumpBuffer();
        // 恢复体力逻辑
        RecoverStamina();
    }
    void FixedUpdate() {
        if (isHurt) return;
        if (isJump) HoldJump();
        // TODO:后续用状态机优化攀爬状态和移动状态的切换
        if (isClimb && physicsCheck.isWall) Climb();
        else Move();
        ClimbUp();
        
    }
    #endregion

    #region 角色移动相关代码
    //实现平滑移动
    void Move() {
        //平滑移动
        Vector2 targetPosition = rb.position + inputDirection * currentSpeed; //目标位置
        Vector2 currentVelocity = rb.velocity;
        Vector2.SmoothDamp(rb.position, targetPosition, ref currentVelocity, smoothTime);
        rb.velocity = new Vector2(currentVelocity.x, rb.velocity.y);
        //调整朝向
        if (rb.velocity.x < -0.1) transform.localScale = new Vector3(-1, 1, 1);
        else if (rb.velocity.x > 0.1) transform.localScale = new Vector3(1, 1, 1);
    }
    #region 跳跃相关代码
    //输入跳跃
    void StartJump(InputAction.CallbackContext context) {
        isJump = true;
        jumpBufferCounter = jumpBufferTime;
    }
    //取消跳跃
    void CancelJump(InputAction.CallbackContext context) {
        isJump = false;
        holdJumpCounter = 0; // 重置跳跃持续时间计时器
    }
    //土狼跳
    void CoyoteJump() {
        if (physicsCheck.isGround)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }
    //跳跃预输入
    void JumpBuffer() {
        if (jumpBufferCounter > 0) {
            jumpBufferCounter -= Time.deltaTime;
            if (physicsCheck.isGround || 
                coyoteTimeCounter > 0 ||
                (isClimb && physicsCheck.isWall)) {
                PerformJump();
                jumpBufferCounter = 0;
            }
        }
    }
    //执行跳跃
    void PerformJump() {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        //蹬墙跳
        if (isClimb && physicsCheck.isWall)
            StartCoroutine(TictacTime()); // 启动蹬墙跳时器

        Vector2 jumpDirection = inputDirection.x == -transform.localScale.x ?
            new Vector2(inputDirection.x, 1) :
            Vector2.up;
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        audioDefination.PlayAudio(0);
        holdJumpCounter = holdJumpTime; // 初始化跳跃持续时间计时器
    }
    //长按跳跃跳得更高
    void HoldJump() {
        if(holdJumpCounter > 0) {
            rb.AddForce(Vector2.up * holdJumpForce, ForceMode2D.Force);
            holdJumpCounter -= Time.fixedDeltaTime; // 减少跳跃持续时间
        }
    }
    //蹬墙跳计时器
    private IEnumerator TictacTime() {
        isClimb=false;
        rb.gravityScale = gravityScale;
        yield return new WaitForSeconds(0.2f);
        //TODO:爬墙状态下跳跃后快速松开攀爬键会导致角色一直处于攀爬状态
        isClimb = true;
    }
    #endregion
    #region 攀爬相关代码
    //输入攀爬
    private void StartClimb(InputAction.CallbackContext context) {
        isClimb = true;
    }
    //取消攀爬
    private void CancelClimb(InputAction.CallbackContext context) {
        isClimb = false;
        rb.gravityScale = gravityScale; // 恢复重力
    }
    //实现攀爬
    void Climb() {
        rb.gravityScale = 0; // 禁用重力
        float verticalInput = inputDirection.y; // 获取垂直输入
        if (currentStamina > 0) {
            rb.velocity = new Vector2(0, verticalInput * climbSpeed);
            if (verticalInput >= 0) 
                currentStamina -= Time.deltaTime; // 消耗体力
        }
        else {
            //TODO:缓慢滑落需要进行墙体检测，否则虚空下滑
            rb.velocity = new Vector2(0, -slideDownSpeed); // 缓慢滑落
        }
    }
    //攀登上平台
    void ClimbUp() {
        if (rb.gravityScale == 0 && isClimb && !physicsCheck.isWall && rb.velocity.y > 0) {
            StartCoroutine(ClimbUpAction());
        }
    }
    IEnumerator ClimbUpAction() {
        isClimb = false; // 取消攀爬状态
        rb.gravityScale = gravityScale; // 恢复重力
        PerformJump();
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(new Vector2(transform.localScale.x,0) * jumpForce * 0.5f, ForceMode2D.Impulse);
        isClimb = true; // 重新进入攀爬状态
    }
    //恢复体力
    void RecoverStamina() {
        if (physicsCheck.isGround) {
            currentStamina = climbStamina; // 恢复体力
        }
    }
    #endregion
    #region 冲刺相关代码
    //冲刺
    void Dash(InputAction.CallbackContext context) {
        if(isDash) return; // 如果正在冲刺，则不执行
        StartCoroutine(DashTime());
        // 获取冲刺方向
        Vector2 dashDirection = inputDirection.Equals(Vector2.zero) ?
            new Vector2(transform.localScale.x, 0) :
            inputDirection;
        // 清空当前速度
        rb.velocity = new Vector2(rb.velocity.x, 0);
        dashDirection = new Vector2(dashDirection.x, dashDirection.y * 0.5f);
        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
        audioDefination.PlayAudio(1);
    }
    //冲刺时间
    private IEnumerator DashTime() {
        isDash = true;
        bool temp = isClimb;
        isClimb = false;
        // 禁用重力
        rb.gravityScale = 0;
        // 冲刺期间禁用移动
        playerInputControl.Gameplay.Move.Disable();
        yield return new WaitForSeconds(dashTime);
        // 启用移动
        playerInputControl.Gameplay.Move.Enable();
        // 恢复重力
        rb.gravityScale = gravityScale;
        isClimb = temp;
        isDash = false;
    }
    #endregion
    //TODO:蹲伏优化
    //void Crouch()
    //{
    //    if(inputDirection.y < 0) //下蹲
    //    {
    //        //TODO:蹲键按下后判断当前速度，如果大于某个值就给予一个推力，实现滑铲效果
    //        isCrouch = true;
    //        currentSpeed = slideSpeed;
    //        col.offset = new Vector2(col.offset.x, -0.5f);
    //        col.size = new Vector2(col.size.x, 1);
    //    }
    //    else if (isCrouch && !physicsCheck.isRoof) //站起
    //    {
    //        isCrouch = false;
    //        currentSpeed = normalSpeed;
    //        col.offset = new Vector2(col.offset.x, -0.3f);
    //        col.size = new Vector2(col.size.x, 1.4f);
    //    }
    //}
    #endregion

    //受伤弹开
    // TODO: 考虑将其移动到Character中？因为受伤触发音效动画是通用逻辑？
    // TODO: 将动画和音效的触发全都放入同一个方法中触发，这样调用受伤只需一次即可，不需要额外调用动画、音效
    public void GetHurt(Vector2 bounceDirection) {
        isHurt = true;
        // TODO: 根据接触点弹开
        // TODO: 受伤时触发音效
        // TODO: 受伤时触发动画
        rb.velocity = Vector2.zero;
        rb.AddForce(bounceDirection.normalized * jumpForce, ForceMode2D.Impulse);
    }
}
