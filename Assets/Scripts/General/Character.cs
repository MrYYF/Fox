using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 用于管理角色数值和数值相关函数的类
/// </summary>
public class Character : MonoBehaviour
{
    #region 参数
    [Header("基础属性")]
    [Tooltip("最大生命值")]public int maxHealth;
    public int currentHealth; //当前生命值
    [Tooltip("伤害")]public int damage;
    [Tooltip("无敌时间")] public float invulnerableDuration;
    float invulnerableCounter; //无敌时间计时器
    bool isInvulnerable;
    [Tooltip("受伤订阅事件")] public UnityEvent<Transform> OnTakeDamage;

    [HideInInspector]public bool isDead;
    #endregion

    #region 生命周期函数
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

    #region 受伤相关
    //受到伤害
    public void TakeDamage(Character attacker)
    {
        if (isInvulnerable || isDead) return; //处于无敌时间

        if (currentHealth - attacker.damage > 0)
        {
            currentHealth = currentHealth - attacker.damage;
            TriggerInvulnerable();
            
            OnTakeDamage?.Invoke(attacker.transform); //启用事件订阅的所有函数 受击音效、受伤位移、人物受伤动画
        }
        else
        {
            currentHealth = 0;
            isDead = true; //死亡判定
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead) return; //处于无敌时间
        if (currentHealth - damage > 0)
        {
            currentHealth = currentHealth - damage;
            TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            isDead = true; //死亡判定
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    //触发无敌状态
    void TriggerInvulnerable()
    {
        isInvulnerable = true;
        invulnerableCounter = invulnerableDuration; //重置计时器
    }
    //无敌时间计时器
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

    //死亡动画后销毁
    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}
