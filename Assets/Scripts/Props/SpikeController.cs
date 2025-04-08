using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpikeController : MonoBehaviour
{
    // TODO: ���ݽӴ��㵯��
    void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(1);
        collision.GetComponent<PlayerController>()?.GetHurt(collision.transform);
        collision.GetComponent<PlayerAnimation>()?.Hurt();
    }
}
