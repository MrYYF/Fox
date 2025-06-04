using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cherry : MonoBehaviour {
    [Tooltip("ʰȡ��Ч")] public AudioClip pickUpSound;
    [Tooltip("��Ƶ�����¼�")] public PlayAudioEventSO playAudioEventSO;
    [Tooltip("�ռ��¼�")]public CollectionEventSO collectionEventSO;
    private Animator animator;
    private Collider2D col;

    private void Awake() {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collectionEventSO?.RaiseEvent(); // �����ռ��¼�
            playAudioEventSO?.RaiseEvent(pickUpSound); // ����ʰȡ��Ч
            animator.SetTrigger("PickUp");
            col.enabled = false; // ��ֹ�ظ�����
        }
    }

    public void OnPickAnimationEnd() {
        Destroy(gameObject); // ����ӣ�Ҷ���
    }

}
