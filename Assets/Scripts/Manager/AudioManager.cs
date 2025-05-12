using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Tooltip("��������")]public AudioSource BGMSource;
    [Tooltip("��Ч")] public AudioSource FXSource;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

}
