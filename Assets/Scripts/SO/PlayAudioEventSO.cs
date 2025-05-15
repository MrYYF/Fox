using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayAudioEvent", menuName = "Event/PlayAudioEventSO")]
public class PlayAudioEventSO : ScriptableObject {
    public UnityAction<AudioClip> OnEventRaised;

    public void RaiseEvent(AudioClip audioClip) {
        OnEventRaised?.Invoke(audioClip);
    }
}