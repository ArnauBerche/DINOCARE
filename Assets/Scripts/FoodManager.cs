using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    int totalFood;
    int remainingFood;

    void Start()
    {
        RecountFoods();
    }

    void RecountFoods()
    {
        var foods = FindObjectsOfType<UIFood>();
        totalFood = Mathf.Max(0, foods.Length);
        remainingFood = totalFood;
    }



    // Conectar desde DropTargetUI.onDropped (pasa el GameObject draggable)
    public void OnFoodDropped(GameObject foodGO)
    {
        if (foodGO == null) return;

        var uiFood = foodGO.GetComponent<UIFood>();
        var draggable = foodGO.GetComponent<DraggableUI>();

        if (uiFood == null)
        {
            // no es comida válida: restaurar
            draggable?.RestoreOriginal();
            return;
        }

        bool eaten = uiFood.TryEat();

        if (eaten)
        {
            remainingFood = Mathf.Max(0, remainingFood - 1);
            Destroy(foodGO);
            if (remainingFood <= 0) EndMinigame();
        }
        else
        {
            // No comido aún (verdura que evade): restaurar visualmente al original
            draggable?.RestoreOriginal();
            // opcional: reproducir feedback (sonido/animación) aquí
        }
    }

    void EndMinigame()
    {
        // Aquí puedes reproducir animación/fx antes de destruir el panel
        Destroy(gameObject);
    }
}