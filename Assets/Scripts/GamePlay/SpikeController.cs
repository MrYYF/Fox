using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpikeController : MonoBehaviour
{
    // TODO: ���ݽӴ��㵯��
    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return; // ֻ������ҽӴ�
        collision.GetComponent<Character>()?.TakeDamage(1);
        collision.GetComponent<PlayerController>()?.GetHurt(Vector2.up);
        collision.GetComponent<PlayerAnimation>()?.Hurt();
    }
}
