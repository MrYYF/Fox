using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Tooltip("±≥æ∞“Ù¿÷")]public AudioSource BGMSource;
    [Tooltip("“Ù–ß")] public AudioSource FXSource;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

}
