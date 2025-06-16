using UnityEngine;
using UnityEngine.AddressableAssets;


/// <summary>
/// ���ڴ洢������Ϣ��ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "Scene", menuName = "Game Scene/GameSceneSO", order = 1)]
public class GameSceneDataSO : ScriptableObject {
    [Header("Basic Information")]
    [Tooltip("��������")]public SceneType scenes;
    [Tooltip("�����ʲ�����")]public AssetReference sceneReference;
}
