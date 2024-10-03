using UnityEngine;

public class PickableItemController : MonoBehaviour
{
    [Tooltip("上下浮动的幅度")]
    public float floatAmplitude = 0.5f;
    [Tooltip("浮动的速度")]
    public float floatSpeed = 4f;
    public ItemFunction itemFunction;
    public enum ItemFunction
    {
        IncreaseJumpCount, //增加跳跃次数
        IncreaseDashCount, //增加冲刺次数
        Heal,  //恢复生命
    }


    private Vector3 startPos;
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private bool isPickedUp = false; // 防止多次拾取

    private void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D=GetComponent<BoxCollider2D>();

        // 记录初始位置
        startPos = transform.position;
    }

    void Update()
    {
        // 计算新的垂直位置
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // 应用新的位置
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !isPickedUp)
        {
            isPickedUp = true;
            PickUp();

            // 触发Feedback动画
            animator.SetTrigger("PickedUp");

            // 禁用碰撞体，防止重复触发
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

        // 这个方法将在动画结束时通过动画事件调用
        public void OnFeedbackAnimationEnd()
    {
        // 销毁游戏对象
        Destroy(gameObject);
    }
}
