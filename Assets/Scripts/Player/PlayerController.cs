using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour {
    #region 
    /**
     * ���� Variables
     */
    [Header("������ֵ")]
    [Tooltip("�ٶȻ�׼ֵ")] public float normalSpeed = 10f;
    float currentSpeed; //��ǰ�ٶ�
    [Tooltip("�ƶ�ƽ��ʱ��")] public float smoothTime = 0.8f;
    [Tooltip("�����ٶ�����")] public float slideSpeed = 8f;
    [Tooltip("�����ٶ�����")] public float climbSpeed = 3f;
    [Tooltip("�����ٶ�")]public float slideDownSpeed = 2f;
    [Tooltip("��������")] public float climbStamina = 5f;
    float currentStamina;
    float gravityScale = 4f; // ����
    [Tooltip("��������")] public float slideForce = 10f;
    [Tooltip("��Ծ����")] public float jumpForce = 10f;
    [Tooltip("������Ծ����")] public float holdJumpForce = 40f;
    [Tooltip("������Ծ����ʱ��")] public float holdJumpTime = 0.15f;
    float holdJumpCounter; // ������Ծ��ʱ��
    [Tooltip("������ʱ��")] public float coyoteTime = 0.1f;
    float coyoteTimeCounter; //��������ʱ��
    [Tooltip("��ԾԤ����ʱ��")] public float jumpBufferTime = 0.1f; // ��ԾԤ����ʱ��
    float jumpBufferCounter; // ��ԾԤ�����ʱ��
    [Tooltip("�������")] public float dashForce = 10f;
    [Tooltip("���ʱ��")] public float dashTime = 0.2f; // ���ʱ��


    /**
     * ���
     */
    PlayerInputControl playerInputControl; //����ϵͳ
    Vector2 inputDirection; //���뷽��
    PhysicsCheck physicsCheck; //��������
    Rigidbody2D rb;
    AudioDefination audioDefination; //��Ч��
    //CapsuleCollider2D col;

    /**
     * ״̬
     */
    [HideInInspector] public bool isJump;
    [HideInInspector] public bool isClimb;
    [HideInInspector] public bool isDash;
    [HideInInspector] public bool isCrouch;
    [HideInInspector] public bool isHurt;

    #endregion

    #region �������ں���
    //��ʼ�����
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
        GameManager.Instance.RigisterPlayer(gameObject); //ע����ҵ���Ϸ������
        currentSpeed = normalSpeed;
        coyoteTimeCounter = coyoteTime;
        gravityScale = rb.gravityScale;
        jumpBufferCounter = 0;
        holdJumpCounter = 0;
    }
    void Update() {
        if (isHurt) return;
        inputDirection = playerInputControl.Gameplay.Move.ReadValue<Vector2>(); //��ȡ�����ƶ�����
        // �������߼�
        CoyoteJump();
        // ��ԾԤ�����߼�
        JumpBuffer();
        // �ָ������߼�
        RecoverStamina();
    }
    void FixedUpdate() {
        if (isHurt) return;
        if (isJump) HoldJump();
        // TODO:������״̬���Ż�����״̬���ƶ�״̬���л�
        if (isClimb && physicsCheck.isWall) Climb();
        else Move();
        ClimbUp();
        
    }
    #endregion

    #region ��ɫ�ƶ���ش���
    //ʵ��ƽ���ƶ�
    void Move() {
        //ƽ���ƶ�
        Vector2 targetPosition = rb.position + inputDirection * currentSpeed; //Ŀ��λ��
        Vector2 currentVelocity = rb.velocity;
        Vector2.SmoothDamp(rb.position, targetPosition, ref currentVelocity, smoothTime);
        rb.velocity = new Vector2(currentVelocity.x, rb.velocity.y);
        //��������
        if (rb.velocity.x < -0.1) transform.localScale = new Vector3(-1, 1, 1);
        else if (rb.velocity.x > 0.1) transform.localScale = new Vector3(1, 1, 1);
    }
    #region ��Ծ��ش���
    //������Ծ
    void StartJump(InputAction.CallbackContext context) {
        isJump = true;
        jumpBufferCounter = jumpBufferTime;
    }
    //ȡ����Ծ
    void CancelJump(InputAction.CallbackContext context) {
        isJump = false;
        holdJumpCounter = 0; // ������Ծ����ʱ���ʱ��
    }
    //������
    void CoyoteJump() {
        if (physicsCheck.isGround)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }
    //��ԾԤ����
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
    //ִ����Ծ
    void PerformJump() {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        //��ǽ��
        if (isClimb && physicsCheck.isWall)
            StartCoroutine(TictacTime()); // ������ǽ��ʱ��

        Vector2 jumpDirection = inputDirection.x == -transform.localScale.x ?
            new Vector2(inputDirection.x, 1) :
            Vector2.up;
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        audioDefination.PlayAudio(0);
        holdJumpCounter = holdJumpTime; // ��ʼ����Ծ����ʱ���ʱ��
    }
    //������Ծ���ø���
    void HoldJump() {
        if(holdJumpCounter > 0) {
            rb.AddForce(Vector2.up * holdJumpForce, ForceMode2D.Force);
            holdJumpCounter -= Time.fixedDeltaTime; // ������Ծ����ʱ��
        }
    }
    //��ǽ����ʱ��
    private IEnumerator TictacTime() {
        isClimb=false;
        rb.gravityScale = gravityScale;
        yield return new WaitForSeconds(0.2f);
        //TODO:��ǽ״̬����Ծ������ɿ��������ᵼ�½�ɫһֱ��������״̬
        isClimb = true;
    }
    #endregion
    #region ������ش���
    //��������
    private void StartClimb(InputAction.CallbackContext context) {
        isClimb = true;
    }
    //ȡ������
    private void CancelClimb(InputAction.CallbackContext context) {
        isClimb = false;
        rb.gravityScale = gravityScale; // �ָ�����
    }
    //ʵ������
    void Climb() {
        rb.gravityScale = 0; // ��������
        float verticalInput = inputDirection.y; // ��ȡ��ֱ����
        if (currentStamina > 0) {
            rb.velocity = new Vector2(0, verticalInput * climbSpeed);
            if (verticalInput >= 0) 
                currentStamina -= Time.deltaTime; // ��������
        }
        else {
            //TODO:����������Ҫ����ǽ���⣬��������»�
            rb.velocity = new Vector2(0, -slideDownSpeed); // ��������
        }
    }
    //�ʵ���ƽ̨
    void ClimbUp() {
        if (rb.gravityScale == 0 && isClimb && !physicsCheck.isWall && rb.velocity.y > 0) {
            StartCoroutine(ClimbUpAction());
        }
    }
    IEnumerator ClimbUpAction() {
        isClimb = false; // ȡ������״̬
        rb.gravityScale = gravityScale; // �ָ�����
        PerformJump();
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(new Vector2(transform.localScale.x,0) * jumpForce * 0.5f, ForceMode2D.Impulse);
        isClimb = true; // ���½�������״̬
    }
    //�ָ�����
    void RecoverStamina() {
        if (physicsCheck.isGround) {
            currentStamina = climbStamina; // �ָ�����
        }
    }
    #endregion
    #region �����ش���
    //���
    void Dash(InputAction.CallbackContext context) {
        if(isDash) return; // ������ڳ�̣���ִ��
        StartCoroutine(DashTime());
        // ��ȡ��̷���
        Vector2 dashDirection = inputDirection.Equals(Vector2.zero) ?
            new Vector2(transform.localScale.x, 0) :
            inputDirection;
        // ��յ�ǰ�ٶ�
        rb.velocity = new Vector2(rb.velocity.x, 0);
        dashDirection = new Vector2(dashDirection.x, dashDirection.y * 0.5f);
        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
        audioDefination.PlayAudio(1);
    }
    //���ʱ��
    private IEnumerator DashTime() {
        isDash = true;
        bool temp = isClimb;
        isClimb = false;
        // ��������
        rb.gravityScale = 0;
        // ����ڼ�����ƶ�
        playerInputControl.Gameplay.Move.Disable();
        yield return new WaitForSeconds(dashTime);
        // �����ƶ�
        playerInputControl.Gameplay.Move.Enable();
        // �ָ�����
        rb.gravityScale = gravityScale;
        isClimb = temp;
        isDash = false;
    }
    #endregion
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
    // TODO: ���ǽ����ƶ���Character�У���Ϊ���˴�����Ч������ͨ���߼���
    // TODO: ����������Ч�Ĵ���ȫ������ͬһ�������д�����������������ֻ��һ�μ��ɣ�����Ҫ������ö�������Ч
    public void GetHurt(Vector2 bounceDirection) {
        isHurt = true;
        // TODO: ���ݽӴ��㵯��
        // TODO: ����ʱ������Ч
        // TODO: ����ʱ��������
        rb.velocity = Vector2.zero;
        rb.AddForce(bounceDirection.normalized * jumpForce, ForceMode2D.Impulse);
    }
}
