using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 用于触发场景加载事件的ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "SenceLoadEvent", menuName = "Event/SenceLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject {
    public UnityAction<GameSceneDataSO> OnEventRaised;

    public void RaiseEvent(GameSceneDataSO gameSceneDataSO) {
        OnEventRaised?.Invoke(gameSceneDataSO);
    }
}
