using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FoodManager : MonoBehaviour
{
    public static FoodManager Singleton;

    void Awake() { if(Singleton == null) Singleton = this; }

    [SerializeField] private Transform foodHolder;
    [SerializeField] private TextMeshProUGUI foodAmountText;

    private int foodAmount;
    
    public int FoodAmount => foodAmount;
    public Transform FoodHolder => foodHolder;

    [SerializeField] private List<GameObject> foodInKitchen = new List<GameObject>();
    public bool HasFoodInKitchen => foodInKitchen.Count > 0;

    public void AddFood(int amount)
    {
        foodAmount += amount;
        foodAmountText.SetText(foodAmount.ToString());
    }

    public void UseFood(int amount)
    {
        foodAmount -= amount;
        foodAmountText.SetText(foodAmount.ToString());
    }

    public void AddFoodInKitchen(GameObject food)
    {
        foodInKitchen.Add(food);
    }

    public void RemoveFoodInKitchen(GameObject food)
    {
        foodInKitchen.Remove(food);
    }

    public void DestroyAllFoodInKitchen()
    {
        foreach(GameObject food in foodInKitchen)
        {
            Destroy(food);
        }
        foodInKitchen.Clear();
    }
}
