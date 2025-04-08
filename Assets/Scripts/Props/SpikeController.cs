using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpikeController : MonoBehaviour
{
    // TODO: 根据接触点弹开
    void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(1);
        collision.GetComponent<PlayerController>()?.GetHurt(collision.transform);
        collision.GetComponent<PlayerAnimation>()?.Hurt();
    }
}
