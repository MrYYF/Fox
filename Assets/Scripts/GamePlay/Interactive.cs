using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * 用于实现按键文字交互
 */
public class Interactive : MonoBehaviour
{
    [Tooltip("进入触发区域时显示的文字")]
    public GameObject interactiveTipText;
    [Tooltip("按下交互键后依次显示的文字列表")]
    public List<GameObject> displayTextList;
    [Tooltip("每段文字显示的时间")]
    public float displayTime = 2f;
    [Tooltip("交互文字的显示是否为一次性")]
    public bool disposable = false;

    bool isInTrigger = false; //是否在触发区域
    bool isDisplayingTextList = false; //是否正在显示文字

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
        if (Input.GetKeyDown(KeyCode.E) && isInTrigger)
        {
            //显示对话
            if(!isDisplayingTextList)
            {
                isDisplayingTextList = true;
                // 启动协程显示UI元素
                StartCoroutine(DisplayTextList());
                interactiveTipText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            if (!isDisplayingTextList)
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

    //协程实现显示文字列表
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

        //一次性显示则在结束时删除
        if (disposable)
        {
            Destroy(gameObject);
        }
    }

}
