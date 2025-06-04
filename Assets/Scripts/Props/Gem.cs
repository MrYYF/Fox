using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class Gem : MonoBehaviour
{
    [Tooltip("��ʧʱ��")] public float respawnDelay = 5f;
    [Tooltip("ʰȡ��Ч")] public AudioClip pickUpSound;
    [Tooltip("��Ƶ�����¼�")]public PlayAudioEventSO playAudioEventSO;
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
            playAudioEventSO.OnEventRaised(pickUpSound); // ����ʰȡ��Ч
            animator.SetTrigger("PickUp");
            col.enabled = false; // ��ֹ�ظ�����
            collision.GetComponent<PlayerController>()?.RecoverDashCount();
        }
    }

    // �ڶ�������ĩβ���¼��������������
    public void OnPickAnimationEnd() {
        sr.color=new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f);
        Invoke(nameof(Respawn), respawnDelay); // ��������³���
    }

    void Respawn() {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        col.enabled = true;
        animator.Play("Gem-Animation", 0, 0f); // ����ΪIdle״̬
    }
}
