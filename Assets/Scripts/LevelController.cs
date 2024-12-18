using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public int levelIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            SceneManager.LoadScene(levelIndex);
        }
    }
}

//TODO：学习序列是为了：游戏开始后的过场动画、吃宝石之后的提示
