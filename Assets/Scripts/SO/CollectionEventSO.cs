using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CollectionEvent", menuName = "Event/CollectionEventSO")]
public class CollectionEventSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
