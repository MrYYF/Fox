using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankController : MonoBehaviour
{
    [Tooltip("�����Ƿ�Ϊһ����")]
    public bool disposable = false;
    [Tooltip("��Ҫ�滻��ͼ�Ķ���")]
    public SpriteRenderer changeSprite;
    public Sprite originSprite;
    public Sprite replaceSprite;
    [Tooltip("��Ҫ������Ч����Ϸ�����б�")]
    public List<GameObject> interactiveGameObjectList;

    bool isInTrigger = false; //�Ƿ��ڴ�������

    // Start is called before the first frame update
    private void Awake()
    {

        if (changeSprite != null)
        {
            changeSprite.sprite = originSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //�滻��ͼ
            if (changeSprite != null)
            {
                //һ���Ի���
                if (disposable)
                {
                    if (changeSprite.sprite == originSprite)
                        StartCoroutine(InteractiveGameObject());
                    changeSprite.sprite = replaceSprite;
                } else
                {
                    changeSprite.sprite = changeSprite.sprite == originSprite ? replaceSprite : originSprite;
                    StartCoroutine(InteractiveGameObject());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isInTrigger = false;
        }
    }

    //��ת���������Ƿ���Ч
    private IEnumerator InteractiveGameObject()
    {
        if (interactiveGameObjectList != null)
        {
            foreach (GameObject gameObject in interactiveGameObjectList)
            {
                Debug.Log(gameObject.name);
                Debug.Log(gameObject.activeSelf);
                if (gameObject != null)
                    gameObject.SetActive(!gameObject.activeSelf);
            }
        }
        yield return null;
    }
}
