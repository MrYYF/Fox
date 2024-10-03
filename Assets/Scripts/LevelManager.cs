using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager LevelManagerInstance;
    public GameObject gameOverUI;

    public bool isGameOver = false;

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

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            gameOverUI.SetActive(isGameOver);
            Time.timeScale = 0;
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
        if (isGameOver)
        {
            isGameOver = false;
            gameOverUI.SetActive(isGameOver);
            PlayerManager.PlayerManagerInstance.Initialization();
            Time.timeScale = 1;
        }
    }

    public void Restart()
    {
        if (isGameOver)
        {
            isGameOver = false;
            gameOverUI.SetActive(isGameOver);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerManager.PlayerManagerInstance.Initialization();
            Time.timeScale = 1;
        }
    }


}
