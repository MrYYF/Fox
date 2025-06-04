using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cherry : MonoBehaviour {
    [Tooltip("拾取音效")] public AudioClip pickUpSound;
    [Tooltip("音频播放事件")] public PlayAudioEventSO playAudioEventSO;
    [Tooltip("收集事件")]public CollectionEventSO collectionEventSO;
    private Animator animator;
    private Collider2D col;

    private void Awake() {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collectionEventSO?.RaiseEvent(); // 触发收集事件
            playAudioEventSO?.RaiseEvent(pickUpSound); // 播放拾取音效
            animator.SetTrigger("PickUp");
            col.enabled = false; // 防止重复触发
        }
    }

    public void OnPickAnimationEnd() {
        Destroy(gameObject); // 销毁樱桃对象
    }

}
