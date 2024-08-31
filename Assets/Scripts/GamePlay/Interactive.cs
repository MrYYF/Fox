using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * ����ʵ�ְ������ֽ���
 */
public class Interactive : MonoBehaviour
{
    [Tooltip("���봥������ʱ��ʾ������")]
    public GameObject interactiveTipText;
    [Tooltip("���½�������������ʾ�������б�")]
    public List<GameObject> displayTextList;
    [Tooltip("ÿ��������ʾ��ʱ��")]
    public float displayTime = 2f;
    [Tooltip("�������ֵ���ʾ�Ƿ�Ϊһ����")]
    public bool disposable = false;

    bool isInTrigger = false; //�Ƿ��ڴ�������
    bool isDisplayingTextList = false; //�Ƿ�������ʾ����

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
        if (Input.GetKeyDown(KeyCode.E) && isInTrigger)
        {
            //��ʾ�Ի�
            if(!isDisplayingTextList)
            {
                isDisplayingTextList = true;
                // ����Э����ʾUIԪ��
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

    //Э��ʵ����ʾ�����б�
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

        //һ������ʾ���ڽ���ʱɾ��
        if (disposable)
        {
            Destroy(gameObject);
        }
    }

}
