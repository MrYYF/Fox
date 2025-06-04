using UnityEngine;
using UnityEngine.UI;

public class Collections : MonoBehaviour
{
    private int cherryCount = 0; // 樱桃数量计数器
    public Text cherryCountText; // UI文本组件，用于显示樱桃数量

    private void Start()
    {
        UpdateCherryCountText(); // 初始化时更新UI文本
    }

    // 增加樱桃数量并更新UI文本
    public void AddCherry()
    {
        cherryCount++;
        UpdateCherryCountText();
    }

    // 更新UI文本显示的樱桃数量
    private void UpdateCherryCountText()
    {
        cherryCountText.text = "X " + cherryCount.ToString();
    }

    // 重置樱桃数量
    public void ResetCherryCount()
    {
        cherryCount = 0;
        UpdateCherryCountText(); // 重置时也更新UI文本
    }

    // 获取当前的樱桃数量
    public int GetCherryCount()
    {
        return cherryCount; // 获取当前的樱桃数量
    }

    // 设置樱桃数量
    public void SetCherryCount(int count)
    {
        cherryCount = count; // 设置樱桃数量
        UpdateCherryCountText(); // 更新UI文本
    }

    
}
