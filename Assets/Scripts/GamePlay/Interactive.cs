using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * ����ʵ�ְ������ֽ���
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
        // ���textList�Ƿ�Ϊ��
        if (displayTextList != null)
        {
            // �����б��е�ÿ��GameObject
            foreach (GameObject textObject in displayTextList)
            {
                // ���GameObject�Ƿ�Ϊ�գ���������ô���
                if (textObject != null)
                {
                    // ��GameObject����Ϊ����Ծ
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
            // ����Э����ʾUIԪ��
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
