using UnityEngine;

/// <summary>
/// ��ҽ�ɫ����������
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    PhysicsCheck physicsCheck;
    PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        animator.SetFloat("velocityX",Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velocityY",rb.velocity.y);
        animator.SetBool("isGround", physicsCheck.isGround);
        animator.SetBool("isCrouch", playerController.isCrouch);
    }

    public void Hurt()
    {
        animator.SetTrigger("hurt");
    }
}