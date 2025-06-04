using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Collections collections; 

    [Header("�¼�����")]
    public CollectionEventSO onCollectionEvent;

    

    private void OnEnable() {
        onCollectionEvent.OnEventRaised += HandleCollectionEvent;
    }

    private void OnDisable() {
        onCollectionEvent.OnEventRaised -= HandleCollectionEvent;
    }

    private void HandleCollectionEvent() {
        collections.AddCherry(); // �����ռ��¼�
    }
}
