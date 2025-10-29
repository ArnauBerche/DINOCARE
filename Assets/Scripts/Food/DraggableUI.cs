using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DinosaurUI targetDinosaur; // opcional, se busca en Awake si es null

    RectTransform rt;
    Canvas canvas;
    CanvasGroup cg;
    Vector2 originalPos;
    bool isDragging;

    public bool IsDragging => isDragging;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        if (targetDinosaur == null)
            targetDinosaur = FindObjectOfType<DinosaurUI>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = rt.anchoredPosition;
        isDragging = true;
        if (cg != null) cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        rt.anchoredPosition += eventData.delta / canvas.scaleFactor;

        var uiFood = GetComponent<UIFood>();
        if (targetDinosaur != null)
        {
            if (uiFood != null) targetDinosaur.NotifyFoodDragged(uiFood);
            else targetDinosaur.NotifyFoodDragged(null);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if (cg != null) cg.blocksRaycasts = true;

        var uiFood = GetComponent<UIFood>();
        if (targetDinosaur != null)
        {
            if (uiFood != null) targetDinosaur.NotifyFoodDragged(uiFood);
            else targetDinosaur.NotifyFoodDragged(null);
        }
    }

    public void RestoreOriginal()
    {
        rt.anchoredPosition = originalPos;
        var uiFood = GetComponent<UIFood>();
        if (targetDinosaur != null && uiFood != null)
            targetDinosaur.NotifyFoodDragged(uiFood);
    }
}