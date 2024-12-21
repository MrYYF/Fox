using UnityEngine;
using UnityEngine.Events;


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
    [Tooltip("受伤订阅事件")] public UnityEvent<Transform> OnTakeDamage;



    private float invulnerableCounter; //无敌时间计时器
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
    
    //受到伤害
    public void TakeDamage(Character attacker)
    {
        if (isInvulnerable) return; //处于无敌时间

        if (currentHealth - attacker.damage > 0)
        {
            currentHealth = currentHealth - attacker.damage;
            TriggerInvulnerable();
            //TODO:受击音效
            //TODO:受伤位移
            //TODO:人物受伤动画
            OnTakeDamage?.Invoke(attacker.transform); //启用事件订阅的所有函数
        }
        else
        {
            currentHealth = 0;
            isDead = true; //死亡判定
        }
    }

    //触发无敌时间
    private void TriggerInvulnerable()
    {
        isInvulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }
}
