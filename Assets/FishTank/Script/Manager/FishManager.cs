using System.Collections.Generic;
using UnityEngine;

public class FishManager : Singleton<FishManager>
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Fish fishPrefab;

    [SerializeField] private List<Fish> fishList;
    [SerializeField] private List<FishData> fishData;
    [SerializeField] private List<SpecsFishByType> fishType;

    [Header("Meta Data")]
    
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float hungerMeterMax = 100f;
    [SerializeField] private float hungerCooldown = 5f;
    [SerializeField] private float avoidanceForce = 5f;
    [SerializeField] private float avoidanceRadius = 1.2f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Fish GotFish()
    {
        foreach (var fish in fishList)
        {
            if (!fish.gameObject.activeInHierarchy)
            {
                return fish;
            }
        }

        return CreateFish();
    }

    public Fish CreateFish()
    {
        Fish newFish = Instantiate(fishPrefab);
        newFish.gameObject.SetActive(false);
        fishList.Add(newFish);
        newFish.InitMetaData(detectionRadius, hungerMeterMax, hungerCooldown, avoidanceForce, avoidanceRadius, minBounds, maxBounds);
        return newFish;
    }

    public FishData RandomFishData()
    {
        int index = Random.Range(0, fishData.Count);
        return fishData[index];
    }

    public SpecsFishByType GetSpecsByType(string newTypeName)
    {
        return fishType.Find(x => x.typeName == newTypeName);
    }
}

[System.Serializable]
public class SpecsFishByType
{
    public string typeName;
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
}