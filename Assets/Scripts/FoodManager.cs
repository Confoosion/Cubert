using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodManager : MonoBehaviour
{
    public static FoodManager Singleton;

    void Awake() { if(Singleton == null) Singleton = this; }

    [SerializeField] private Transform foodHolder;
    [SerializeField] private TextMeshProUGUI foodAmountText;

    private int foodAmount;
    
    public int FoodAmount => foodAmount;
    public Transform FoodHolder => foodHolder;

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
}
