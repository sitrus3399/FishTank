using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private FoodData foodData;
    [SerializeField] private float foodValue;

    public float FoodValue { get { return foodValue; } }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitData(FoodData newFood)
    {
        foodData = newFood;
    }
}