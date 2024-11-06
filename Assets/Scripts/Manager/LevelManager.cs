using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager LevelManagerInstance;
    public GameObject gameOverUI;
    public GameObject gamePauseUI;

    [Header("状态")]
    public bool isGameOver = false;
    public bool isGamePause = false;
    public bool canPause = false;

    void Awake()
    {
        gameOverUI.SetActive(false);
        if (LevelManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        LevelManagerInstance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //判断是否在不可暂停界面
            if (SceneManager.GetActiveScene().buildIndex == 0)
                canPause = false;
            else
                canPause = true;

            if (!isGameOver && canPause)
                GamePause();
        }
    }

    //游戏暂停
    public void GamePause()
    {
        if (!isGamePause) //进入游戏暂停状态
        {
            isGamePause = true;
            Time.timeScale = 0;
            gamePauseUI.SetActive(true);
            PlayerManager.PlayerManagerInstance.canMove = false; //禁用玩家操作
        }
        else if (isGamePause) //退出游戏暂停状态
        {
            isGamePause = false;
            Time.timeScale = 1f;
            gamePauseUI.SetActive(false);
            PlayerManager.PlayerManagerInstance.canMove = true;
        }
    }

    //游戏结束
    public void GameOver()
    {
        isGameOver = true;
        isGamePause = true;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
        PlayerManager.PlayerManagerInstance.canMove = false;
    }

    //返回主菜单
    public void ToMenu()
    {
        isGameOver = false;
        isGamePause = false;
        gameOverUI.SetActive(false);
        gamePauseUI.SetActive(false);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        PlayerManager.PlayerManagerInstance.Initialization();
    }

    // 重新开始
    public void Restart()
    {
        isGameOver = false;
        isGamePause = false;
        gameOverUI.SetActive(false);
        gamePauseUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        PlayerManager.PlayerManagerInstance.Initialization();
    }


}
