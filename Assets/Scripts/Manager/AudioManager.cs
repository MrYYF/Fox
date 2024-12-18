using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioManagerInstance;


    private void Awake()
    {
        if (AudioManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        AudioManagerInstance = this;
    }
}
