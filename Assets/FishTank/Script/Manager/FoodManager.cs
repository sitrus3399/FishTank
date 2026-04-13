using System.Collections.Generic;
using UnityEngine;

public class FoodManager : Singleton<FoodManager>
{
    [SerializeField] private GameEvent gameEvent;

    [SerializeField] private Food foodPrefab;
    [SerializeField] private List<Food> foodList;

    [SerializeField] private FoodData selectedFood;

    public FoodData SelectedFood { get { return selectedFood; } }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Food GotFood()
    {
        foreach (var trash in foodList)
        {
            if (!trash.gameObject.activeInHierarchy)
            {
                return trash;
            }
        }

        return CreateFood();
    }

    public Food CreateFood()
    {
        Food newFood = Instantiate(foodPrefab);
        newFood.gameObject.SetActive(false);
        foodList.Add(newFood);
        //Food.InitMetaData(minSpeed, maxSpeed, avoidanceForce, avoidanceRadius, minBounds, maxBounds);
        return newFood;
    }
}
