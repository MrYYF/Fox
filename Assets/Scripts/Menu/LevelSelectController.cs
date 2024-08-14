using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        // ��ȡ��ť������
        string buttonName = button.name;

        // ����ť������ת��Ϊ����
        if (int.TryParse(buttonName, out int levelIndex))
        {
            // ���ض�Ӧ�����ĳ���
            SceneManager.LoadScene(levelIndex);
        }
    }
}
