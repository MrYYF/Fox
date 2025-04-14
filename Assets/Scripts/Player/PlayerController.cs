using System;
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
    [Tooltip("速度基准值")] public float normalSpeed;
    float currentSpeed; //当前速度
    [Tooltip("移动平滑时间")] public float smoothTime;
    [Tooltip("滑铲速度修正")] public float slideSpeed;
    [Tooltip("滑铲推力")] public float slideForce;
    [Tooltip("跳跃推力")] public float jumpForce;
    [Tooltip("持续跳跃推力")] public float holdJumpForce;
    [Tooltip("持续跳跃推力时间")] public float holdJumpTime; // 持续跳跃时间
    float holdJumpCounter; // 持续跳跃计时器
    [Tooltip("土狼跳时间")] public float coyoteTime;
    float coyoteTimeCounter; //土狼跳计时器
    [Tooltip("跳跃预输入时间")] public float jumpBufferTime; // 跳跃预输入时间
    float jumpBufferCounter; // 跳跃预输入计时器
    
    [Tooltip("冲刺推力")] public float dashForce;


    /**
     * 组件
     */
    PlayerInputControl playerInputControl; //输入系统
    Vector2 inputDirection; //输入方向
    PhysicsCheck physicsCheck; //物理检测类
    Rigidbody2D rb;
    //CapsuleCollider2D col;

    /**
     * 状态
     */
    [HideInInspector] public bool isJump;
    [HideInInspector] public bool isCrouch;
    [HideInInspector] public bool isHurt;

    #endregion

    #region 生命周期函数
    //初始化组件
    void Awake() {
        GameManager.Instance.RigisterPlayer(gameObject); //注册玩家到游戏管理器

        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerInputControl = new PlayerInputControl();
        playerInputControl.Gameplay.Jump.started += StartJump;
        playerInputControl.Gameplay.Jump.canceled += CancelJump;
        playerInputControl.Gameplay.Dash.started += Dash;
    }

    void OnEnable() {
        playerInputControl?.Enable();
    }
    void OnDisable() {
        playerInputControl?.Disable();
    }
    void Start() {
        currentSpeed = normalSpeed;
        coyoteTimeCounter = coyoteTime;
    }
    void Update() {
        if (isHurt) return;
        inputDirection = playerInputControl.Gameplay.Move.ReadValue<Vector2>(); //获取输入移动向量

        // 土狼跳逻辑
        if (physicsCheck.isGround) 
            coyoteTimeCounter = coyoteTime;
        else 
            coyoteTimeCounter -= Time.deltaTime;
        

        // 跳跃预输入逻辑
        if (jumpBufferCounter > 0) {
            jumpBufferCounter -= Time.deltaTime;
            if (physicsCheck.isGround || coyoteTimeCounter > 0) {
                PerformJump();
                jumpBufferCounter = 0;
            }
        }
    }
    void FixedUpdate() {
        if (isHurt) return;
        if (isJump) HoldJump();
        Move();
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
        if (currentVelocity.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        if (currentVelocity.x > 0) transform.localScale = new Vector3(1, 1, 1);
    }
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
    //执行跳跃
    void PerformJump() {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        holdJumpCounter = holdJumpTime; // 初始化跳跃持续时间计时器
    }
    //长按跳跃跳得更高
    void HoldJump() {
        if(holdJumpCounter > 0) {
            rb.AddForce(Vector2.up * holdJumpForce, ForceMode2D.Force);
            holdJumpCounter -= Time.fixedDeltaTime; // 减少跳跃持续时间
        }
    }
    //冲刺
    void Dash(InputAction.CallbackContext context) {
        rb.AddForce(inputDirection * dashForce, ForceMode2D.Impulse);
    }
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
    public void GetHurt(Transform attacker) {
        isHurt = true;
        // TODO: 根据接触点弹开
        rb.velocity = Vector2.zero;
        Vector2 direction = new Vector2(transform.position.x - attacker.position.x, 0.3f).normalized;
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
    }
}
