using UnityEngine;

public class UIFood : MonoBehaviour
{
    public enum FoodType { Normal, Vegetable }

    public FoodType foodType = FoodType.Normal;
    [Tooltip("Cuántos intentos necesarios para que Dini acepte la verdura")]
    public int attemptsToFeed = 3;

    int attemptsLeft;

    void Start()
    {
        attemptsLeft = Mathf.Max(1, attemptsToFeed);
    }

    // Devuelve true si la comida se considera "comida" ahora
    public bool TryEat()
    {
        if (foodType == FoodType.Vegetable)
        {
            attemptsLeft--;
            return attemptsLeft <= 0;
        }
        return true;
    }
}