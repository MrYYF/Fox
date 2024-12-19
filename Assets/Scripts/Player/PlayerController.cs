using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region ���� Variables
    [Header("������ֵ")]
    [Tooltip("�ٶȻ�׼ֵ")] public float speed;
    float currentSpeed; //��ǰ�ٶ�
    [Tooltip("�ƶ�ƽ��ʱ��")] public float smoothTime;
    [Tooltip("�����ٶ�����")] public float slideSpeedModify; //TODO:���������ó���һ���ٶȺ��¶׻�ʩ������ͬʱ�����ٶ����޵ķ�����ʵ��
    [Tooltip("��������")] public float slideForce;
    [Tooltip("��Ծ����")] public float jumpForce;
    [Tooltip("�������")] public float dashForce;

    PlayerInputControl playerInputControl; //����ϵͳ
    Vector2 inputDirection; //���뷽��
    PhysicsCheck physicsCheck; //��������
    Rigidbody2D rb;
    CapsuleCollider2D col;

    //����״̬
    [HideInInspector]public bool isCrouch;
    
    #endregion

    #region �������ں���
    //��ʼ�����
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
        inputDirection = playerInputControl.Gameplay.Move.ReadValue<Vector2>(); //��ȡ�����ƶ�����
        Crouch();
    }
    void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region ��ɫ�ƶ���ش���
    //ʵ��ƽ���ƶ�
    void Move()
    {
        //ƽ���ƶ�
        Vector2 targetPosition = rb.position + inputDirection * currentSpeed; //Ŀ��λ��
        Vector2 currentVelocity = rb.velocity;
        Vector2.SmoothDamp(rb.position, targetPosition, ref currentVelocity, smoothTime);
        rb.velocity = new Vector2(currentVelocity.x, rb.velocity.y);
        //��������
        if(currentVelocity.x < 0) transform.localScale = new Vector3( -1, 1, 1);
        if(currentVelocity.x > 0) transform.localScale = new Vector3( 1, 1, 1);
    }
    //��Ծ
    void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        ////�ڵ���
        //if (isGround)
        //{
        //    isJump = false;
        //    //ResetAbilityCount();
        //} else if (!isJump)
        //{
        //    isJump = true;
        //    jumpCount--;
        //}

        //if (isJumpPressed) //������Ծ
        //{
        //    isJump = true;
        //    isJumpPressed = false;
        //    rb.velocity = new Vector2(rb.velocity.x, 0);
        //    if (isCrouch && isGround)
        //        rb.AddForce(Vector2.up * jumpForce * 1.5f, ForceMode2D.Impulse); //�¶���
        //    else
        //    jumpCount--;
        //}
    }
    //���
    void Dash(InputAction.CallbackContext context)
    {
        rb.AddForce(inputDirection * dashForce, ForceMode2D.Impulse);
    }
    //TODO:�׷�
    void Crouch()
    {
        if(inputDirection.y < 0) //�¶�
        {
            isCrouch = true;
            currentSpeed = speed * slideSpeedModify;
            col.offset = new Vector2(col.offset.x, -0.5f);
            col.size = new Vector2(col.size.x, 1);
        }
        else if (isCrouch && !physicsCheck.isRoof) //վ��
        {
            isCrouch = false;
            currentSpeed = speed;
            col.offset = new Vector2(col.offset.x, -0.3f);
            col.size = new Vector2(col.size.x, 1.4f);
        }
    }

    //TODO:������
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
