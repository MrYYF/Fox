using UnityEngine;

/// <summary>
/// 玩家角色动画控制类
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    PhysicsCheck physicsCheck;
    PlayerController playerController;
    Character character;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
        character = GetComponent<Character>();
    }
    private void Update()
    {
        animator.SetFloat("velocityX",Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velocityY",rb.velocity.y);
        animator.SetBool("isGround", physicsCheck.isGround);
        animator.SetBool("isCrouch", playerController.isCrouch);
        animator.SetBool("isDead", character.isDead);
    }

    // TODO: 考虑将其移动到Character中？因为受伤触发音效动画是通用逻辑
    public void Hurt()
    {
        // TODO: 受伤时触发音效
        animator.SetTrigger("hurt");
    }
}
