using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¼ì²éÈËÎï×´Ì¬
/// </summary>
public class PhysicsCheck : MonoBehaviour
{
    [Header("¶¥²¿¼ì²â")]
    [Tooltip("¶¥²¿¼ì²âºÐ³ß´ç")] public Vector2 roofCheckSize;
    [Tooltip("¶¥²¿¼ì²âºÐÆ«ÒÆ")] public Vector2 roofCheckoffset;
    public bool isRoof;

    [Header("µØÃæ¼ì²â")]
    [Tooltip("µØÃæ¼ì²âºÐ³ß´ç")]public Vector2 groundCheckSize;
    [Tooltip("µØÃæ¼ì²âºÐÆ«ÒÆ")] public Vector2 groundCheckoffset;
    public bool isGround;

    [Header("²àÃæ¼ì²é")]
    [Tooltip("Ç½±Ú¼ì²âÉäÏß·½Ïò")] public Vector2 wallCheckDirection;
    [Tooltip("Ç½±Ú¼ì²âÉäÏß³¤¶È")] public float wallCheckLength;
    [Tooltip("Ç½±Ú¼ì²âºÐÆ«ÒÆ")] public Vector2 wallCheckoffset;
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
        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckoffset, groundCheckSize); //µØÃæ¼ì²â
        Gizmos.DrawWireCube((Vector2)transform.position + roofCheckoffset, roofCheckSize); //¶¥²¿¼ì²â
        Gizmos.DrawRay((Vector2)transform.position + wallCheckoffset, wallCheckDirection.normalized * wallCheckLength); //Ç½±Ú¼ì²â
    }
}
