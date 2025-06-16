using UnityEngine;
using UnityEngine.AddressableAssets;


/// <summary>
/// 用于存储场景信息的ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "Scene", menuName = "Game Scene/GameSceneSO", order = 1)]
public class GameSceneDataSO : ScriptableObject {
    [Header("Basic Information")]
    [Tooltip("场景类型")]public SceneType scenes;
    [Tooltip("场景资产引用")]public AssetReference sceneReference;
}
