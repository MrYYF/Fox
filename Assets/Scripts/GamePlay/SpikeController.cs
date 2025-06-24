using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpikeController : MonoBehaviour
{
    // TODO: 根据接触点弹开
    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return; // 只处理玩家接触
        collision.GetComponent<Character>()?.TakeDamage(1);
        collision.GetComponent<PlayerController>()?.GetHurt(Vector2.up);
        collision.GetComponent<PlayerAnimation>()?.Hurt();
    }
}
