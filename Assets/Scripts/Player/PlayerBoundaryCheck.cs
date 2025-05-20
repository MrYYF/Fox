using UnityEngine;

public class PlayerBoundaryCheck : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.GetComponent<Character>()?.TakeDamage(1);
            collision.GetComponent<PlayerController>()?.GetHurt(Vector2.up);
            collision.GetComponent<PlayerAnimation>()?.Hurt();
        }
    }
}
