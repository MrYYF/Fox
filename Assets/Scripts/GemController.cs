using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GemController : MonoBehaviour
{
    [Tooltip("���¸����ķ���")]
    public float floatAmplitude = 0.5f;
    [Tooltip("�������ٶ�")]
    public float floatSpeed = 4f;

    
    private Vector3 startPos;
    private Animator animator;
    private bool isPickedUp = false; // ��ֹ���ʰȡ

    private void Start()
    {
        animator = GetComponent<Animator>();

        // ��¼��ʼλ��
        startPos = transform.position;
    }

    void Update()
    {
        // �����µĴ�ֱλ��
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Ӧ���µ�λ��
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !isPickedUp)
        {
            isPickedUp = true;

            // ����Feedback����
            animator.SetTrigger("PickedUp");

            // ������ײ�壬��ֹ�ظ�����
            GetComponent<Collider2D>().enabled = false;
        }
    }

    // ����������ڶ�������ʱͨ�������¼�����
    public void OnFeedbackAnimationEnd()
    {
        // ������Ϸ����
        Destroy(gameObject);
    }
}
