using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("�����¼�")]
    [Tooltip("��Ч�¼�")] public PlayAudioEventSO FXEvent;
    [Tooltip("���������¼�")] public PlayAudioEventSO BGMEvent;
    [Header("��Ч���")]
    [Tooltip("��Ч")] public AudioSource FXSource;
    [Tooltip("��������")]public AudioSource BGMSource;

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
