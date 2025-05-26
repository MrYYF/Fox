using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class Gem : MonoBehaviour
{
    Animator animator;

    private void Reset() {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    private void Awake() {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            animator.SetTrigger("PickUp");
            collision.GetComponent<PlayerController>()?.RecoverDashCount();
        }
    }
}
