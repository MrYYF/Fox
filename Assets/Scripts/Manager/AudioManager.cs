using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("监听事件")]
    [Tooltip("音效事件")] public PlayAudioEventSO FXEvent;
    [Tooltip("背景音乐事件")] public PlayAudioEventSO BGMEvent;
    [Header("音效组件")]
    [Tooltip("音效")] public AudioSource FXSource;
    [Tooltip("背景音乐")]public AudioSource BGMSource;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
    }

    private void OnDisable() {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
    }

    private void OnFXEvent(AudioClip audioClip) {
        FXSource.clip = audioClip;
        FXSource.Play();
    }
    private void OnBGMEvent(AudioClip audioClip) {
        BGMSource.clip = audioClip;
        BGMSource.Play();
    }



}
