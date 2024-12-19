using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 用于管理角色数值和数值相关函数的类
/// </summary>
public class Character : MonoBehaviour
{
    [Header("基础属性")]
    [Tooltip("最大生命值")]public int maxHealth;
    [Tooltip("当前生命值")]public int currentHealth;
    [Tooltip("伤害")]public int damage;
    [Tooltip("无敌时间")] public float invulnerableDuration;


    private float invulnerableCounter; //无敌时间计时器
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
    
    //受到伤害
    public void TakeDamage(Character attacker)
    {
        if (isInvulnerable) return;
        TriggerInvulnerable();
        currentHealth = Mathf.Max(currentHealth - attacker.damage, 0);

        if (currentHealth == 0) isDead = true;
    }

    //触发无敌时间
    private void TriggerInvulnerable()
    {
        isInvulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }
}
