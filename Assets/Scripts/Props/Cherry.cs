using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour {
    private Animator animator;
    private Collider2D col;

    private void Awake() {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            animator.SetTrigger("PickUp");
            col.enabled = false; // ��ֹ�ظ�����
        }
    }

    public void OnPickAnimationEnd() {
        Destroy(gameObject); // ����ӣ�Ҷ���
    }

}
