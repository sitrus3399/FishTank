using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    private float currentCooldownSpawner;

    [SerializeField] private Vector2 minRange;
    [SerializeField] private Vector2 maxRange;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void SpawnFood()
    {
        Food newFood = FoodManager.Instance.GotFood();

        if (newFood != null)
        {
            float randomX = Random.Range(minRange.x, maxRange.x);
            float randomY = Random.Range(minRange.y, maxRange.y);
            Vector3 randomPosition = new Vector3(randomX, randomY, 0);

            newFood.transform.position = randomPosition;
            newFood.gameObject.SetActive(true);
            newFood.InitData(FoodManager.Instance.SelectedFood);
        }
    }
}