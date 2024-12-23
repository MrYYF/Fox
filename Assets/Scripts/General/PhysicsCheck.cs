using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������״̬
/// </summary>
public class PhysicsCheck : MonoBehaviour
{
    [Header("�������")]
    [Tooltip("�������гߴ�")] public Vector2 roofCheckSize;
    [Tooltip("��������ƫ��")] public Vector2 roofCheckoffset;
    public bool isRoof;

    [Header("������")]
    [Tooltip("������гߴ�")]public Vector2 groundCheckSize;
    [Tooltip("�������ƫ��")] public Vector2 groundCheckoffset;
    public bool isGround;

    [Header("������")]
    [Tooltip("ǽ�ڼ�����߷���")] public Vector2 wallCheckDirection;
    [Tooltip("ǽ�ڼ�����߳���")] public float wallCheckLength;
    [Tooltip("ǽ�ڼ���ƫ��")] public Vector2 wallCheckoffset;
    public bool isWall;

    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GroundCheck();
        RoofCheck();
        WallCheck();
    }
    void GroundCheck()
    {
        isGround = Physics2D.OverlapBox((Vector2)transform.position + groundCheckoffset, groundCheckSize, 0, LayerMask.GetMask("Ground"));
    }
    void RoofCheck()
    {
        isRoof = Physics2D.OverlapBox((Vector2)transform.position + roofCheckoffset, roofCheckSize, 0, LayerMask.GetMask("Ground"));
    }
    void WallCheck()
    {
        wallCheckDirection = new Vector2(transform.localScale.x, 0);
        isWall = Physics2D.Raycast((Vector2)transform.position + wallCheckoffset, wallCheckDirection, wallCheckLength, LayerMask.GetMask("Ground"));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckoffset, groundCheckSize); //������
        Gizmos.DrawWireCube((Vector2)transform.position + roofCheckoffset, roofCheckSize); //�������
        Gizmos.DrawRay((Vector2)transform.position + wallCheckoffset, wallCheckDirection.normalized * wallCheckLength); //ǽ�ڼ��
    }
}
