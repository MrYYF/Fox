using System;
using System.Collections;
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
    [Tooltip("�����ٶ�����")] public float slideSpeedModify;
    [Tooltip("��������")] public float slideForce;
    [Tooltip("��Ծ����")] public float jumpForce;
    [Tooltip("������ʱ��")] public float coyoteTime; //������ʱ��
    public float coyoteTimeCounter;

    
    [Tooltip("�������")] public float dashForce;

    PlayerInputControl playerInputControl; //����ϵͳ
    Vector2 inputDirection; //���뷽��
    PhysicsCheck physicsCheck; //��������
    Rigidbody2D rb;
    CapsuleCollider2D col;

    //����״̬
    //public bool isJump;
    [HideInInspector]public bool isCrouch;
    [HideInInspector]public bool isHurt;

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
    void Start()
    {
        currentSpeed = speed;
        coyoteTimeCounter = coyoteTime;
    }
    void Update()
    {
        if (isHurt) return;

        inputDirection = playerInputControl.Gameplay.Move.ReadValue<Vector2>(); //��ȡ�����ƶ�����
        Crouch();

        //TODO:������
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
        {
            //isJump = true;
            //coyoteTimeCounter = -1;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //TODO:������
    }
    //���
    void Dash(InputAction.CallbackContext context)
    {
        rb.AddForce(inputDirection * dashForce, ForceMode2D.Impulse);
    }
    //TODO:�׷��Ż�
    void Crouch()
    {
        if(inputDirection.y < 0) //�¶�
        {
            //TODO:�׼����º��жϵ�ǰ�ٶȣ��������ĳ��ֵ�͸���һ��������ʵ�ֻ���Ч��
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
    

    #endregion

    //���˵���
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;

        Vector2 direction = new Vector2(transform.position.x - attacker.position.x,0.3f).normalized;
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
    }
}
