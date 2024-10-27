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

    [Header("״̬")]
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
            //�ж��Ƿ��ڲ�����ͣ����
            if (SceneManager.GetActiveScene().buildIndex == 0)
                canPause = false;
            else
                canPause = true;

            if (!isGameOver && canPause)
                GamePause();
        }
    }

    //��Ϸ��ͣ
    public void GamePause()
    {
        if (!isGamePause) //������Ϸ��ͣ״̬
        {
            isGamePause = true;
            Time.timeScale = 0;
            gamePauseUI.SetActive(true);
        }
        else if (isGamePause) //�˳���Ϸ��ͣ״̬
        {
            isGamePause = false;
            Time.timeScale = 1f;
            gamePauseUI.SetActive(false);
        }
    }

    //��Ϸ����
    public void GameOver()
    {
        isGameOver = true;
        isGamePause = true;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    //�������˵�
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

    // ���¿�ʼ
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
