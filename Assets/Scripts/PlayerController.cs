using System.Collections;
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
    public Vector2 moveDirection;

    int jumpCount;
    int dashCount;

    new Rigidbody2D rigidbody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;
    private bool isjump = false;
    

    //��ʼ�����
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpCount = MainManager.Instance.maxJumpCount;
        dashCount = MainManager.Instance.maxDashCount;
    }

    void Update()
    {
        MoveDirection();
        AnimationController();
        CharacterMovement();
        Dash();
    }

    //��ײ������
    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.CompareTag("Ground"))
    //    {
    //        ResetAbilityCount();
    //    }
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (rigidbody2D.velocity.y == 0f && isjump)
            {
                ResetAbilityCount();
                isjump = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (!isjump)
                StartCoroutine(CoyoteTime());
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

    void AnimationController()
    {
        if (moveDirection.x != 0)
        {
            animator.SetBool("IsRunning", true);
            if (moveDirection.x < 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        } else
            animator.SetBool("IsRunning", false);


        if (rigidbody2D.velocity.y > 0)
            animator.SetBool("IsJumping", true);
        else if(rigidbody2D.velocity.y==0 && moveDirection.y < 0)
            animator.SetBool("IsCrouching", true);
        else if (rigidbody2D.velocity.y < 0)
            animator.SetBool("IsFall", true);
        else if (rigidbody2D.velocity.y == 0)
        {
            animator.SetBool("IsFall", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsCrouching", false);
        }

        animator.SetFloat("ySpeed", rigidbody2D.velocity.y);
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

    //������
    private IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(0.1f);
        if (jumpCount > 0)
        {
            jumpCount--;
            isjump = true;
        }
            
    }












}
