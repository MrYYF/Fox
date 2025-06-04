using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Collections collections; 

    [Header("事件监听")]
    public CollectionEventSO onCollectionEvent;

    

    private void OnEnable() {
        onCollectionEvent.OnEventRaised += HandleCollectionEvent;
    }

    private void OnDisable() {
        onCollectionEvent.OnEventRaised -= HandleCollectionEvent;
    }

    private void HandleCollectionEvent() {
        collections.AddCherry(); // 触发收集事件
    }
}
