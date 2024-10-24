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
        UpdateHealthUI(maxHitPoint);
    }

    // ����Ѫ��UI
    public void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < healthIconList.Count; i++)
        {
            if (i < currentHealth)
                healthIconList[i].enabled = true; // ��ʾͼ��
            else
                healthIconList[i].enabled = false; // ����ͼ��
        }
    }

    
}
