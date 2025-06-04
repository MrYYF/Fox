using UnityEngine;
using UnityEngine.UI;

public class Collections : MonoBehaviour
{
    private int cherryCount = 0; // ӣ������������
    public Text cherryCountText; // UI�ı������������ʾӣ������

    private void Start()
    {
        UpdateCherryCountText(); // ��ʼ��ʱ����UI�ı�
    }

    // ����ӣ������������UI�ı�
    public void AddCherry()
    {
        cherryCount++;
        UpdateCherryCountText();
    }

    // ����UI�ı���ʾ��ӣ������
    private void UpdateCherryCountText()
    {
        cherryCountText.text = "X " + cherryCount.ToString();
    }

    // ����ӣ������
    public void ResetCherryCount()
    {
        cherryCount = 0;
        UpdateCherryCountText(); // ����ʱҲ����UI�ı�
    }

    // ��ȡ��ǰ��ӣ������
    public int GetCherryCount()
    {
        return cherryCount; // ��ȡ��ǰ��ӣ������
    }

    // ����ӣ������
    public void SetCherryCount(int count)
    {
        cherryCount = count; // ����ӣ������
        UpdateCherryCountText(); // ����UI�ı�
    }

    
}
