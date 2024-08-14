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
        // 获取按钮的名称
        string buttonName = button.name;

        // 将按钮的名称转换为整数
        if (int.TryParse(buttonName, out int levelIndex))
        {
            // 加载对应索引的场景
            SceneManager.LoadScene(levelIndex);
        }
    }
}
