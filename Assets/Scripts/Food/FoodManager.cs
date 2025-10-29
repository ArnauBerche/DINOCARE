using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ResetFood(UIFood food)
    {
        if (food == null) return;
        food.Restore();
    }

    public void EatFood(UIFood food, DinosaurUI dino)
    {
        if (food == null)
        {
            if (dino != null) dino.NotifyAteFoodComplete();
            TryDisableParentCanvasIfNoFoodLeft(null);
            return;
        }

        // efectos, puntuación, animaciones, etc. pueden añadirse aquí.

        // Comprobar si tras comer esta comida no queda ninguna otra activa
        TryDisableParentCanvasIfNoFoodLeft(food);

        Destroy(food.gameObject);

        // Notificar al dinosaurio que la acción de comer terminó para que pueda reaccionar de nuevo
        if (dino != null) dino.NotifyAteFoodComplete();
    }

    public void GiveFoodToDinosaur(UIFood food, DinosaurUI dino)
    {
        if (food == null || dino == null) return;
        EatFood(food, dino);
    }

    // Comprueba las UIFood activas en la escena y desactiva el Canvas padre si no queda ninguna.
    void TryDisableParentCanvasIfNoFoodLeft(UIFood eatenFood)
    {
        // Cuenta las UIFood activas en la escena
        var foods = FindObjectsOfType<UIFood>();
        int totalActive = foods != null ? foods.Length : 0;

        // Si se está comiendo una comida concreta, restarla de la cuenta
        if (eatenFood != null) totalActive = Mathf.Max(0, totalActive - 1);

        if (totalActive == 0)
        {
            var parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                parentCanvas.gameObject.SetActive(false);
            }
        }
    }
}