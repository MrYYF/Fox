using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// ���ڹ����ɫ��ֵ����ֵ��غ�������
/// </summary>
public class Character : MonoBehaviour
{
    #region ����
    [Header("��������")]
    [Tooltip("�������ֵ")]public int maxHealth;
    public int currentHealth; //��ǰ����ֵ
    [Tooltip("�˺�")]public int damage;
    [Tooltip("�޵�ʱ��")] public float invulnerableDuration;
    float invulnerableCounter; //�޵�ʱ���ʱ��
    bool isInvulnerable;
    [Tooltip("���˶����¼�")] public UnityEvent<Transform> OnTakeDamage;

    [HideInInspector]public bool isDead;
    #endregion

    #region �������ں���
    void Start()
    {
        currentHealth = maxHealth;
        isInvulnerable = false;
        isDead = false;
    }
    void Update()
    {
        InvulnerableCounter();
    }
    #endregion

    #region �������
    //�ܵ��˺�
    public void TakeDamage(Character attacker)
    {
        if (isInvulnerable || isDead) return; //�����޵�ʱ��

        if (currentHealth - attacker.damage > 0)
        {
            currentHealth = currentHealth - attacker.damage;
            TriggerInvulnerable();
            
            OnTakeDamage?.Invoke(attacker.transform); //�����¼����ĵ����к��� �ܻ���Ч������λ�ơ��������˶���
        }
        else
        {
            currentHealth = 0;
            isDead = true; //�����ж�
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead) return; //�����޵�ʱ��
        if (currentHealth - damage > 0)
        {
            currentHealth = currentHealth - damage;
            TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            isDead = true; //�����ж�
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    //�����޵�״̬
    void TriggerInvulnerable()
    {
        isInvulnerable = true;
        invulnerableCounter = invulnerableDuration; //���ü�ʱ��
    }
    //�޵�ʱ���ʱ��
    void InvulnerableCounter()
    {
        if (isInvulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
                isInvulnerable = false;
        }
    }
    #endregion

    //��������������
    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}
