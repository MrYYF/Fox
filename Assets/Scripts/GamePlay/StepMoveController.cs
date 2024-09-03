using UnityEngine;

/**
 * ����ʵ���ƶ�ƽ̨����
 */
[RequireComponent(typeof(Rigidbody2D))]
public class StepMoveController : MonoBehaviour
{
    public Vector2 end;
    public float speed =2f;
    
    Vector2 start;
    Vector2 target; // ��ǰĿ���
    bool direction = true;
    Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        target = end;
        start = transform.position;
    }

    
    void FixedUpdate()
    {
        // �������嵱ǰλ�õ�Ŀ���ľ���
        float step = speed * Time.fixedDeltaTime; // ÿ֡�ƶ��ľ���
        Vector2 newPosition = Vector2.MoveTowards(rigidbody2D.position, target, step);
        //Vector2 newPosition = Vector2.SmoothDamp(rb.position, target, ref velocity, smoothTime, speed, Time.fixedDeltaTime);
        rigidbody2D.MovePosition(newPosition);

        // ��������Ƿ񵽴�Ŀ���
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            // �л�Ŀ���
            direction = !direction;
            target = direction ? end : start;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Rigidbody2D playerRigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                playerRigidbody2D.velocity = playerRigidbody2D.velocity + rigidbody2D.velocity;
            }
        }
    }
}
