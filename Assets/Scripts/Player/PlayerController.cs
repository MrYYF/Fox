using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region 
    /**
     * ���� Variables
     */
    [Header("������ֵ")]
    [Tooltip("�ٶȻ�׼ֵ")] public float normalSpeed;
    float currentSpeed; //��ǰ�ٶ�
    [Tooltip("�ƶ�ƽ��ʱ��")] public float smoothTime;
    [Tooltip("�����ٶ�����")] public float slideSpeed;
    [Tooltip("��������")] public float slideForce;
    [Tooltip("��Ծ����")] public float jumpForce;
    [Tooltip("������ʱ��")] public float coyoteTime;
    float coyoteTimeCounter; //��������ʱ��
    [Tooltip("��ԾԤ����ʱ��")] public float jumpBufferTime; // ��ԾԤ����ʱ��
    float jumpBufferCounter; // ��ԾԤ�����ʱ��
    [Tooltip("�������")] public float dashForce;
    

    /**
     * ���
     */
    PlayerInputControl playerInputControl; //����ϵͳ
    Vector2 inputDirection; //���뷽��
    PhysicsCheck physicsCheck; //��������
    Rigidbody2D rb;
    //CapsuleCollider2D col;

    /**
     * ״̬
     */
    //public bool isJump;
    [HideInInspector] public bool isCrouch;
    [HideInInspector] public bool isHurt;

    #endregion

    #region �������ں���
    //��ʼ�����
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        //col = GetComponent<CapsuleCollider2D>();
        playerInputControl = new PlayerInputControl();
        playerInputControl.Gameplay.Jump.started += Jump;
        //playerInputControl.Gameplay.Jump.canceled += Bounce;
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
        currentSpeed = normalSpeed;
        coyoteTimeCounter = coyoteTime;
    }
    void Update()
    {
        if (isHurt) return;
        inputDirection = playerInputControl.Gameplay.Move.ReadValue<Vector2>(); //��ȡ�����ƶ�����

        // �������߼�
        if (physicsCheck.isGround)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // ��ԾԤ�����߼�
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
            if (physicsCheck.isGround || coyoteTimeCounter > 0)
            {
                PerformJump();
                jumpBufferCounter = 0;
            }
        }
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
        if (currentVelocity.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        if (currentVelocity.x > 0) transform.localScale = new Vector3(1, 1, 1);
    }
    //������Ծ����
    void Jump(InputAction.CallbackContext context)
    {
        jumpBufferCounter = jumpBufferTime;
    }
    //ִ����Ծ
    void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    //TODO:С��
    //void Bounce(InputAction.CallbackContext context)
    //{
    //    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
    //}

    //���
    void Dash(InputAction.CallbackContext context)
    {
        rb.AddForce(inputDirection * dashForce, ForceMode2D.Impulse);
    }
    //TODO:�׷��Ż�
    //void Crouch()
    //{
    //    if(inputDirection.y < 0) //�¶�
    //    {
    //        //TODO:�׼����º��жϵ�ǰ�ٶȣ��������ĳ��ֵ�͸���һ��������ʵ�ֻ���Ч��
    //        isCrouch = true;
    //        currentSpeed = slideSpeed;
    //        col.offset = new Vector2(col.offset.x, -0.5f);
    //        col.size = new Vector2(col.size.x, 1);
    //    }
    //    else if (isCrouch && !physicsCheck.isRoof) //վ��
    //    {
    //        isCrouch = false;
    //        currentSpeed = normalSpeed;
    //        col.offset = new Vector2(col.offset.x, -0.3f);
    //        col.size = new Vector2(col.size.x, 1.4f);
    //    }
    //}
    #endregion

    //���˵���
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        // TODO: ���ݽӴ��㵯��
        rb.velocity = Vector2.zero;
        Vector2 direction = new Vector2(transform.position.x - attacker.position.x, 0.3f).normalized;
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
    }
}
