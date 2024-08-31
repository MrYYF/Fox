using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankController : MonoBehaviour
{
    [Tooltip("交互是否为一次性")]
    public bool disposable = false;
    [Tooltip("需要替换贴图的对象")]
    public SpriteRenderer changeSprite;
    public Sprite originSprite;
    public Sprite replaceSprite;
    [Tooltip("需要控制生效的游戏对象列表")]
    public List<GameObject> interactiveGameObjectList;

    bool isInTrigger = false; //是否在触发区域

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
            //替换贴图
            if (changeSprite != null)
            {
                //一次性或复用
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

    //反转交互物体是否生效
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
