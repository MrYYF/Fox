using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

[DefaultExecutionOrder(-1)]
public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public int maxJumpCount=2;
    public int maxDashCount=1;
    public int hp = 3;

    private void Awake()
    {
        //Initialization();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Initialization()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

}
