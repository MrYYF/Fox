using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 ÓÃÓÚ¼ì²éÈËÎïµÄ×´Ì¬
 */
public class PhysicsCheck : MonoBehaviour
{
    [Header("µØÃæ¼ì²â")]
    [Tooltip("µØÃæ¼ì²âºÐ³ß´ç")]public Vector2 groundCheckSize;
    [Tooltip("µØÃæ¼ì²âºÐÆ«ÒÆ")] public Vector2 groundCheckoffset;
    public bool isGround;

    [Header("¶¥²¿¼ì²â")]
    [Tooltip("¶¥²¿¼ì²âºÐ³ß´ç")] public Vector2 roofCheckSize;
    [Tooltip("¶¥²¿¼ì²âºÐÆ«ÒÆ")] public Vector2 roofCheckoffset;
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
        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckoffset, groundCheckSize); //µØÃæ¼ì²â
        Gizmos.DrawWireCube((Vector2)transform.position + roofCheckoffset, roofCheckSize); //¶¥²¿¼ì²â
    }
}
