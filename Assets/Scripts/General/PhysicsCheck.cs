using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 ���ڼ�������״̬
 */
public class PhysicsCheck : MonoBehaviour
{
    [Header("������")]
    [Tooltip("������гߴ�")]public Vector2 groundCheckSize;
    [Tooltip("�������ƫ��")] public Vector2 groundCheckoffset;
    public bool isGround;

    [Header("�������")]
    [Tooltip("�������гߴ�")] public Vector2 roofCheckSize;
    [Tooltip("��������ƫ��")] public Vector2 roofCheckoffset;
    public bool isRoof;
    void Update()
    {
        GroundCheck();
        RoofCheck();
    }
    void GroundCheck()
    {
        isGround = Physics2D.OverlapBox((Vector2)transform.position + groundCheckoffset, groundCheckSize, 0, LayerMask.GetMask("Ground"));
    }
    void RoofCheck()
    {
        isRoof = Physics2D.OverlapBox((Vector2)transform.position + roofCheckoffset, roofCheckSize, 0, LayerMask.GetMask("Ground"));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckoffset, groundCheckSize); //������
        Gizmos.DrawWireCube((Vector2)transform.position + roofCheckoffset, roofCheckSize); //�������
    }
}
