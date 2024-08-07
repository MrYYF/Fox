using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[RequireComponent(typeof(Rigidbody2D))]
public class StepMoveController : MonoBehaviour
{
    public Vector2 end;
    public float speed =2f;
    
    Vector2 start;
    Vector2 target; // ��ǰĿ���
    bool direction = true;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = end;
        start = transform.position;
    }

    void Update()
    {
        // �������嵱ǰλ�õ�Ŀ���ľ���
        float step = speed * Time.fixedDeltaTime; // ÿ֡�ƶ��ľ���
        Vector2 newPosition = Vector2.MoveTowards(rb.position, target, step);
        //Vector2 newPosition = Vector2.SmoothDamp(rb.position, target, ref velocity, smoothTime, speed, Time.fixedDeltaTime);
        rb.MovePosition(newPosition);



        // ��������Ƿ񵽴�Ŀ���
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            // �л�Ŀ���
            direction = !direction;
            target = direction ? end : start;
        }
    }


}
