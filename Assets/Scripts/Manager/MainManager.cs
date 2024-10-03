using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

[DefaultExecutionOrder(-1)]
public class MainManager : MonoBehaviour
{
    public static MainManager MainManagerInstance;

    private void Awake()
    {
        Initialization();
        
        DontDestroyOnLoad(gameObject);
    }

    void Initialization()
    {
        if (MainManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        MainManagerInstance = this;
    }

}
