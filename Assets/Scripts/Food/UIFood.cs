using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIFood : MonoBehaviour, IPointerClickHandler
{
    public bool isVegetable = false;
    public DraggableUI draggable; // opcional, se busca en Awake
    public Image visualImage; // opcional: referencia a la imagen para cambiar color/icono

    RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (draggable == null) draggable = GetComponent<DraggableUI>();
        if (visualImage == null) visualImage = GetComponentInChildren<Image>();
        UpdateVisual();
    }

    public Vector2 AnchoredPosition => rt != null ? rt.anchoredPosition : Vector2.zero;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (draggable != null && draggable.IsDragging) return;
        isVegetable = !isVegetable;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (visualImage == null) return;
        visualImage.color = isVegetable ? new Color(0.6f, 1f, 0.6f) : Color.white;
    }

    public void Restore()
    {
        if (draggable != null) draggable.RestoreOriginal();
    }
}