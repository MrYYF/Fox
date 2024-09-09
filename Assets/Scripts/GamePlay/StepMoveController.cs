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
    Rigidbody2D playerRigidbody2D;

    float step; // ÿ֡�ƶ��ľ���
    Vector2 newPosition;
    Vector2 platformMovement;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        target = end;
        start = transform.position;
    }

    
    void FixedUpdate()
    {
        // �������嵱ǰλ�õ�Ŀ���ľ���
        step = speed * Time.fixedDeltaTime; // ÿ֡�ƶ��ľ���
        newPosition = Vector2.MoveTowards(rigidbody2D.position, target, step);
        platformMovement = newPosition - rigidbody2D.position;
        rigidbody2D.MovePosition(newPosition);
        if (playerRigidbody2D != null)
        {
            //playerRigidbody2D.MovePosition(playerRigidbody2D.position+platformMovement);
            playerRigidbody2D.position += platformMovement;
        }

        // ��������Ƿ񵽴�Ŀ���
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            // �л�Ŀ���
            direction = !direction;
            target = direction ? end : start;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                playerRigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        playerRigidbody2D = null;
    }
}
