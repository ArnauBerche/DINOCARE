using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { }

public class DropTargetUI : MonoBehaviour, IDropHandler
{
    public GameObjectEvent onDropped;

    public void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;
        if (go == null) return;

        // Reparent opcional: colocar dentro del target
        go.transform.SetParent(transform, false);
        var rt = go.GetComponent<RectTransform>();
        if (rt != null) rt.anchoredPosition = Vector2.zero;

        onDropped?.Invoke(go);
    }
}