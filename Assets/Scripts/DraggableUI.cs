using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent { get; private set; }
    public Vector2 originalAnchoredPos { get; private set; }

    Canvas canvas;
    RectTransform rect;
    CanvasGroup canvasGroup;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalAnchoredPos = rect.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        if (canvas != null) transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (transform.parent == canvas.transform)
        {
            RestoreOriginal();
        }
    }

    // Llamable desde manager para devolver la pieza a su lugar
    public void RestoreOriginal()
    {
        transform.SetParent(originalParent, false);
        rect.anchoredPosition = originalAnchoredPos;
    }
}