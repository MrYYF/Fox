using System;
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
    private void Start()
    {
        currentSpeed = speed;
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


    
}
