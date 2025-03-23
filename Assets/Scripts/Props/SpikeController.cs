using UnityEngine;

public class SpikeController : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(1);
        collision.GetComponent<PlayerController>()?.GetHurt(transform);
        collision.GetComponent<PlayerAnimation>()?.Hurt();
    }
}
