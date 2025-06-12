using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class Gem : MonoBehaviour
{
    [Tooltip("消失时间")] public float respawnDelay = 5f;
    [Tooltip("拾取音效")] public AudioClip pickUpSound;
    [Tooltip("音频播放事件")]public PlayAudioEventSO playAudioEventSO;
    private Animator animator;
    private Collider2D col;
    private SpriteRenderer sr;

    private void Reset() {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            playAudioEventSO.OnEventRaised(pickUpSound); // 播放拾取音效
            animator.SetTrigger("PickUp");
            col.enabled = false; // 防止重复触发
            collision.GetComponent<PlayerController>()?.RecoverDashCount();
        }
    }

    // 在动画剪辑末尾加事件，调用这个方法
    public void OnPickAnimationEnd() {
        sr.color=new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f);
        Invoke(nameof(Respawn), respawnDelay); // 几秒后重新出现
    }

    void Respawn() {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        col.enabled = true;
        animator.Play("Gem-Animation", 0, 0f); // 重置为Idle状态
    }
}
