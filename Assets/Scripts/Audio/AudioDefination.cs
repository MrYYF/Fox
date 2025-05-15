using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public List<AudioClip> audioClip;
    public PlayAudioEventSO playAudioEventSO;

    public void PlayAudio(int index) {
        playAudioEventSO.RaiseEvent(audioClip[index]);
    }
}
