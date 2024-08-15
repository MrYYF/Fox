using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * 用于实现按键文字交互
 */
public class Interactive : MonoBehaviour
{
    public GameObject interactiveTipText;
    public List<GameObject> displayTextList;
    public float displayTime = 2f;

    bool isInTrigger = false;
    bool isDisplayingTextList = false;

    private void Awake()
    {
        // 检查textList是否为空
        if (displayTextList != null)
        {
            // 遍历列表中的每个GameObject
            foreach (GameObject textObject in displayTextList)
            {
                // 检查GameObject是否不为空，避免空引用错误
                if (textObject != null)
                {
                    // 将GameObject设置为不活跃
                    textObject.SetActive(false);
                }
            }
        }

        if (interactiveTipText != null)
        {
            interactiveTipText.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInTrigger && !isDisplayingTextList)
        {
            isDisplayingTextList = true;
            // 启动协程显示UI元素
            StartCoroutine(DisplayTextList());
            interactiveTipText.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            interactiveTipText.SetActive(true);
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            interactiveTipText.SetActive(false);
            isInTrigger = false;
        }
    }

    private IEnumerator DisplayTextList()
    {
        if(displayTextList != null)
        {
            foreach (GameObject gameObject in displayTextList)
            {
                if (gameObject != null)
                {
                    gameObject.SetActive(true);
                    yield return new WaitForSeconds(displayTime);
                    gameObject.SetActive(false);
                }
            }
        }
        isDisplayingTextList = false;
        interactiveTipText.SetActive(true);
    }
}
