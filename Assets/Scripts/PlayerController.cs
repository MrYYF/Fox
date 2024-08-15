using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("�ٶ�")]
    public float speed = 10f; //�ƶ��ٶ�
    [Tooltip("��Ծ����")]
    public float jumpForce = 10f;
    [Tooltip("�������")]
    public float dashForce = 3f;

    public float smoothTime = 1f;
    public Vector2 currentVelocity;

    int jumpCount;
    int dashCount;

    new Rigidbody2D rigidbody2D;
    public Vector2 moveDirection;


    //��ʼ�����
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        jumpCount = MainManager.Instance.maxJumpCount;
        dashCount = MainManager.Instance.maxDashCount;
    }

    void Update()
    {
        MoveDirection();
        CharacterMovement();
        Dash();
    }

    //��ײ������
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            ResetAbilityCount();
        }
    }

    //������������ƶ�����
    void MoveDirection()
    {
        if (Input.GetKey(KeyCode.A))
            moveDirection.x = -1;
        else if (Input.GetKey(KeyCode.D))
            moveDirection.x = 1;
        else if (Input.GetKey(KeyCode.W))
            moveDirection.y = 1;
        else if (Input.GetKey(KeyCode.S))
            moveDirection.y = -1;
        else
            moveDirection = Vector2.zero;

        moveDirection.Normalize();
    }

    //ʵ��ƽ���ƶ�
    void CharacterMovement()
    {
        // ��ǰ�����λ��
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition + moveDirection * speed;
        targetPosition.y=currentPosition.y;

        // ƽ���ƶ���Ŀ��λ��
        Vector2 newPosition = Vector2.SmoothDamp(
            currentPosition,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );

        // ��������λ��
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        
        if (jumpCount>0 && Input.GetKeyDown(KeyCode.W))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount --;
        }
    }

    //���
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashCount>0)
        {
            rigidbody2D.AddForce(moveDirection * dashForce , ForceMode2D.Impulse);
            dashCount --;
        }
    }

    //�����ƶ����ܴ���
    void ResetAbilityCount()
    {
        jumpCount = MainManager.Instance.maxJumpCount;
        dashCount = MainManager.Instance.maxDashCount;
    }
    











}
