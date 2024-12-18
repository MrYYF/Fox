using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager LevelManagerInstance;
    public GameObject gameOverUI;
    public GameObject gamePauseUI;
    public GameObject gameOptionsUI;

    [Header("״̬")]
    public bool isGameOver = false;
    public bool isGamePause = false;
    public bool isGameOptions = false;
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
            if (SceneManager.GetActiveScene().buildIndex == 0 && isGameOver)
                canPause = false;
            else
                canPause = true;

            //esc���˵��ϼ��˵�/�����˵�
            if (canPause)
                if (!isGameOptions && !isGamePause) //δ���κν���
                    GamePause();
                else if (isGamePause) //�Ѵ���ͣ�˵�
                    if (isGameOptions)
                        GameOptions();
                    else
                        GamePause();
        }
    }

    //��Ϸ��ͣ����
    public void GamePause()
    {
        if (!isGamePause) //������Ϸ��ͣ״̬
        {
            isGamePause = true;
            Time.timeScale = 0;
            gamePauseUI.SetActive(true);
            PlayerManager.PlayerManagerInstance.canMove = false; //������Ҳ���
        }
        else if (isGamePause) //�˳���Ϸ��ͣ״̬
        {
            isGamePause = false;
            Time.timeScale = 1f;
            gamePauseUI.SetActive(false);
            PlayerManager.PlayerManagerInstance.canMove = true;
        }
    }

    //��Ϸ���ý���
    public void GameOptions()
    {
        if (!isGameOptions && isGamePause)
        {
            isGameOptions = true;
            gameOptionsUI.SetActive(true);
            gamePauseUI.SetActive(false);
        } 
        else if (isGameOptions) //�˳���Ϸ���ý���
        {
            isGameOptions = false;
            gameOptionsUI.SetActive(false);
            gamePauseUI.SetActive(true);
        }
    }

    //��Ϸ��������
    public void GameOver()
    {
        isGameOver = true;
        isGamePause = true;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
        PlayerManager.PlayerManagerInstance.canMove = false;
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
