using UnityEngine;
using UnityEngine.Events;


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
    [Tooltip("���˶����¼�")] public UnityEvent<Transform> OnTakeDamage;



    private float invulnerableCounter; //�޵�ʱ���ʱ��
    private bool isInvulnerable;
    [HideInInspector]public bool isDead;

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
        if (isInvulnerable) return; //�����޵�ʱ��

        if (currentHealth - attacker.damage > 0)
        {
            currentHealth = currentHealth - attacker.damage;
            TriggerInvulnerable();
            //TODO:�ܻ���Ч
            //TODO:����λ��
            //TODO:�������˶���
            OnTakeDamage?.Invoke(attacker.transform); //�����¼����ĵ����к���
        }
        else
        {
            currentHealth = 0;
            isDead = true; //�����ж�
        }
    }

    //�����޵�ʱ��
    private void TriggerInvulnerable()
    {
        isInvulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }
}
