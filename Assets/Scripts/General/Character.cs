using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���ڹ����ɫ��ֵ����ֵ��غ�������
/// </summary>
public class Character : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("�������ֵ")]public int maxHealth;
    [Tooltip("��ǰ����ֵ")]public int currentHealth;
    [Tooltip("�˺�")]public int damage;
    [Tooltip("�޵�ʱ��")] public float invulnerableDuration;


    private float invulnerableCounter; //�޵�ʱ���ʱ��
    private bool isInvulnerable;
    public bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;
        isInvulnerable = false;
        isDead = false;
    }

    private void Update()
    {
        if (isInvulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0) 
                isInvulnerable = false;
        }
    }
    
    //�ܵ��˺�
    public void TakeDamage(Character attacker)
    {
        if (isInvulnerable) return;
        TriggerInvulnerable();
        currentHealth = Mathf.Max(currentHealth - attacker.damage, 0);

        if (currentHealth == 0) isDead = true;
    }

    //�����޵�ʱ��
    private void TriggerInvulnerable()
    {
        isInvulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }
}
