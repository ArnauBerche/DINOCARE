using UnityEngine;

public class DinosaurUI : MonoBehaviour
{
    public float detectionRadius = 100f;
    public float detectionMultiplier = 1.5f;
    public float maxEscapeDistance = 200f;
    public float escapeSpeed = 250f;
    public float reachDistance = 30f;

    RectTransform rt;
    Vector2 startPos;
    bool beingChased;
    Vector2 lastFoodPos;
    UIFood currentFood;
    bool hasEaten;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
    }

    void Update()
    {
        if (beingChased && !hasEaten && currentFood != null && currentFood.isVegetable)
        {
            lastFoodPos = currentFood.AnchoredPosition;
            float dist = Vector2.Distance(lastFoodPos, rt.anchoredPosition);

            if (dist <= reachDistance)
            {
                Eat(currentFood);
                return;
            }

            if (dist > detectionRadius * detectionMultiplier)
            {
                beingChased = false;
                currentFood = null;
                return;
            }

            Vector2 dir = (rt.anchoredPosition - lastFoodPos);
            if (dir == Vector2.zero) dir = Vector2.up * 0.01f;
            dir.Normalize();

            Vector2 desired = rt.anchoredPosition + dir * escapeSpeed * Time.deltaTime;

            Vector2 fromStart = desired - startPos;
            if (fromStart.magnitude > maxEscapeDistance)
            {
                desired = startPos + fromStart.normalized * maxEscapeDistance;
            }

            rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, desired, escapeSpeed * Time.deltaTime);
        }
    }

    // Llamado por DraggableUI con la referencia completa a la comida
    public void NotifyFoodDragged(UIFood food)
    {
        if (food == null)
        {
            beingChased = false;
            currentFood = null;
            return;
        }

        currentFood = food;
        lastFoodPos = food.AnchoredPosition;
        float dist = Vector2.Distance(lastFoodPos, rt.anchoredPosition);

        if (!food.isVegetable)
        {
            // Comida normal: no huye, se come si está al alcance
            beingChased = false;
            if (dist <= reachDistance)
            {
                Eat(food);
            }
            return;
        }

        // Verduras: comportamiento de huida
        if (dist <= reachDistance)
        {
            Eat(food);
            return;
        }

        beingChased = dist <= detectionRadius * detectionMultiplier;
    }

    void Eat(UIFood food)
    {
        if (hasEaten) return;
        hasEaten = true;
        beingChased = false;
        currentFood = null;

        if (FoodManager.Instance != null)
            FoodManager.Instance.EatFood(food, this);
        else
            Destroy(food.gameObject);
    }

    // Llamar desde FoodManager cuando la comida ya ha sido procesada (destruida o reiniciada)
    public void NotifyAteFoodComplete()
    {
        hasEaten = false;
    }

    public void ResetToStart()
    {
        hasEaten = false;
        beingChased = false;
        currentFood = null;
        rt.anchoredPosition = startPos;
    }
}