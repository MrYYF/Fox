using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager PlayerManagerInstance;
    PlayerController player;

    public int maxJumpCount = 2;
    public int maxDashCount = 1;
    public int maxHitPoint = 3;

    public List<Image> healthIconList;
    int currentHealth;

    public bool isInvulnerable = false; // �Ƿ����޵�״̬
    public float invulnerabilityDuration = 1f;

    void Awake()
    {
        if (PlayerManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        PlayerManagerInstance = this;
        Initialization();
    }

    public void Initialization()
    {
        if (GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }
        isInvulnerable = false;
        currentHealth = maxHitPoint;
        UpdateHealthUI();
    }

    // ����Ѫ��UI
    public void UpdateHealthUI()
    {
        for (int i = 0; i < healthIconList.Count; i++)
        {
            if (i < currentHealth)
                healthIconList[i].enabled = true; // ��ʾͼ��
            else
                healthIconList[i].enabled = false; // ����ͼ��
        }
    }

    // ������ܵ��˺�ʱ����
    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            currentHealth -= damage;
            UpdateHealthUI();
            if (currentHealth <= 0)
            {
                currentHealth = 0;  // ��ֹѪ��С��0
                LevelManager.LevelManagerInstance.GameOver();
                return;
            }

            if (player == null)
            {
                if (GameObject.Find("Player") != null)
                {
                    player = GameObject.Find("Player").GetComponent<PlayerController>();
                }
            }
                StartCoroutine(player.FlashCoroutine());
        }
    }
}
