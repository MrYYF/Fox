using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家控制类
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region 变量 Variables
    [Header("基础数值")]
    [Tooltip("速度基准值")] public float speed;
    float currentSpeed; //当前速度
    [Tooltip("移动平滑时间")] public float smoothTime;
    [Tooltip("滑铲速度修正")] public float slideSpeedModify;
    [Tooltip("滑铲推力")] public float slideForce;
    [Tooltip("跳跃推力")] public float jumpForce;
    [Tooltip("土狼跳时间")] public float coyoteTime; //土狼跳时间
    public float coyoteTimeCounter;

    
    [Tooltip("冲刺推力")] public float dashForce;

    PlayerInputControl playerInputControl; //输入系统
    Vector2 inputDirection; //输入方向
    PhysicsCheck physicsCheck; //物理检测类
    Rigidbody2D rb;
    CapsuleCollider2D col;

    //人物状态
    //public bool isJump;
    [HideInInspector]public bool isCrouch;
    [HideInInspector]public bool isHurt;

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
    void Start()
    {
        currentSpeed = speed;
        coyoteTimeCounter = coyoteTime;
    }
    void Update()
    {
        if (isHurt) return;

        inputDirection = playerInputControl.Gameplay.Move.ReadValue<Vector2>(); //获取输入移动向量
        Crouch();

        //TODO:土狼跳
        //if (physicsCheck.isGround && !isJump)
        //{
        //    coyoteTimeCounter = coyoteTime;
        //}
        //else if (!physicsCheck.isGround && isJump)
        //{
        //    coyoteTimeCounter -= Time.deltaTime;
        //}
        //else if (coyoteTimeCounter < 0 && isJump)
        //{
        //    isJump = false;
        //}
    }
    void FixedUpdate()
    {
        if (isHurt) return;
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
        {
            //isJump = true;
            //coyoteTimeCounter = -1;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //TODO:二段跳
    }
    //冲刺
    void Dash(InputAction.CallbackContext context)
    {
        rb.AddForce(inputDirection * dashForce, ForceMode2D.Impulse);
    }
    //TODO:蹲伏优化
    void Crouch()
    {
        if(inputDirection.y < 0) //下蹲
        {
            //TODO:蹲键按下后判断当前速度，如果大于某个值就给予一个推力，实现滑铲效果
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
    

    #endregion

    //受伤弹开
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;

        Vector2 direction = new Vector2(transform.position.x - attacker.position.x,0.3f).normalized;
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
    }
}
