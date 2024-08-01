using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    [Tooltip("速度")]
    public float speed = 10f; //移动速度
    [Tooltip("跳跃推力")]
    public float jumpForce = 7f;
    [Tooltip("冲刺推力")]
    public float dashForce = 5f;

    public int jumpCount = 2;
    public int dashCount = 2;

    new Rigidbody2D rigidbody2D;
    public Vector2 moveDirection;


    //初始化组件
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveDirection();
        CharacterMovement();
        Dash();
    }

    void MoveDirection()
    {
        if (Input.GetKey(KeyCode.A))
            moveDirection = Vector2.left;
        else if (Input.GetKey(KeyCode.D))
            moveDirection = Vector2.right;
        else if (Input.GetKey(KeyCode.W))
            moveDirection = Vector2.up;
        else if (Input.GetKey(KeyCode.S))
            moveDirection = Vector2.down;
        else
            moveDirection = Vector2.zero;
    }

    void CharacterMovement()
    {
        if (moveDirection.x != 0)
        {
            transform.Translate(moveDirection * speed * Time.deltaTime);
        }

        if (jumpCount>0 && Input.GetKeyDown(KeyCode.W))
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount --;
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashCount>0)
        {
            rigidbody2D.AddForce(moveDirection * dashForce , ForceMode2D.Impulse);
            dashCount --;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            jumpCount = 2;
            dashCount = 2;
        }
    }
    











}
