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

    public void Hurt()
    {
        animator.SetTrigger("hurt");
    }
}
