using UnityEngine;

public class PickableItemController : MonoBehaviour
{
    [Tooltip("���¸����ķ���")]
    public float floatAmplitude = 0.5f;
    [Tooltip("�������ٶ�")]
    public float floatSpeed = 4f;
    public ItemFunction itemFunction;
    public enum ItemFunction
    {
        IncreaseJumpCount, //������Ծ����
        IncreaseDashCount, //���ӳ�̴���
        Heal,  //�ָ�����
    }


    private Vector3 startPos;
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private bool isPickedUp = false; // ��ֹ���ʰȡ

    private void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D=GetComponent<BoxCollider2D>();

        // ��¼��ʼλ��
        startPos = transform.position;
    }

    void Update()
    {
        // �����µĴ�ֱλ��
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Ӧ���µ�λ��
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !isPickedUp)
        {
            isPickedUp = true;
            PickUp();

            // ����Feedback����
            animator.SetTrigger("PickedUp");

            // ������ײ�壬��ֹ�ظ�����
            boxCollider2D.enabled = false;

            GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    private void PickUp()
    {
        if (isPickedUp)
        {
            switch (itemFunction)
            {
                case ItemFunction.IncreaseJumpCount:
                    PlayerManager.PlayerManagerInstance.maxJumpCount++;
                    break;
                case ItemFunction.IncreaseDashCount:
                    PlayerManager.PlayerManagerInstance.maxDashCount++;
                    break;
                case ItemFunction.Heal:
                    PlayerManager.PlayerManagerInstance.maxHitPoint++;
                    break;
            }
        }
    }

        // ����������ڶ�������ʱͨ�������¼�����
        public void OnFeedbackAnimationEnd()
    {
        // ������Ϸ����
        Destroy(gameObject);
    }
}
