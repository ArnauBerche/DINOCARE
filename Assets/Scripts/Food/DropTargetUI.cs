using UnityEngine;
using UnityEngine.EventSystems;

public class DropTargetUI : MonoBehaviour, IDropHandler
{
    public bool acceptDrop = false; // si true trata el drop como "dar comida"
    public DinosaurUI targetDinosaur; // opcional, asignar en inspector

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null || eventData.pointerDrag == null) return;

        var draggedObj = eventData.pointerDrag;
        var draggable = draggedObj.GetComponent<DraggableUI>();
        var uiFood = draggedObj.GetComponent<UIFood>();

        // Si aceptamos drops y tenemos FoodManager + dino + comida, delegamos
        if (acceptDrop && uiFood != null && targetDinosaur != null && FoodManager.Instance != null)
        {
            FoodManager.Instance.GiveFoodToDinosaur(uiFood, targetDinosaur);
            return;
        }

        // Si no es un drop válido, restaurar la posición original
        if (draggable != null)
        {
            draggable.RestoreOriginal();
        }
        else if (uiFood != null)
        {
            uiFood.Restore();
        }
    }
}