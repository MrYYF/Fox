using UnityEngine;

/// <summary>
/// ���˿��ƽű�
/// </summary>
public class EnemyController : MonoBehaviour
{
    //TODO:״̬��
    //TODO:��ͬ����ģʽ��Ѳ��ģʽ��׷��
    [Header("��������")]
    [Tooltip("�ٶȻ�׼ֵ")]public float normalSpeed;
    float currentSpeed;
    [Tooltip("׷���ٶ�")]public float chaseSpeed;
    [Tooltip("�ȴ�ʱ��")]public float stayTime;
    float stayTimeCounter;
    bool isStay;

    Rigidbody2D rb;
    Vector2 faceDirection;
    PhysicsCheck physicsCheck;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        stayTimeCounter = stayTime;
        isStay = false;
        currentSpeed = normalSpeed;
        faceDirection = new Vector2(transform.localScale.x, 0);
    }
    void Update()
    {
        if (isStay)
        {
            stayTimeCounter -= Time.deltaTime;
            if (stayTimeCounter < 0)
            {
                isStay = false;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }
        }
        else if (physicsCheck.isWall && !isStay)
        {
            stayTimeCounter = stayTime;
            isStay = true;
        }

    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (isStay)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        //ƽ���ƶ�
        faceDirection = new Vector2(transform.localScale.x, 0);
        Vector2 targetPosition = rb.position + faceDirection * currentSpeed; //Ŀ��λ��
        Vector2 currentVelocity = rb.velocity;
        Vector2.SmoothDamp(rb.position, targetPosition, ref currentVelocity, 1);
        rb.velocity = new Vector2(currentVelocity.x, rb.velocity.y);
    }

    //�����˺��ж���Χ�ܵ��˺�
    void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(GetComponent<Character>());
    }

    
}
