using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class FishManager : Singleton<FishManager>
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private SaveEvent saveEvent;
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

    protected override void Awake()
    {
        base.Awake();
        LoadDataFromConfig();
    }

    private void OnEnable()
    {
        gameEvent.OnAddFish += AddDataFish;
        gameEvent.OnRemoveFishData += RemoveDataFish;
    }

    private void OnDisable()
    {
        gameEvent.OnAddFish -= AddDataFish;
        gameEvent.OnRemoveFishData -= RemoveDataFish;
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

    void LoadDataFromConfig()
    {
        saveEvent.OnLoadSpecsFishByType += (value) => { fishType = value; };
        saveEvent.OnLoadFishDetectionRadius += (value) => { detectionRadius = value; };
        saveEvent.OnLoadFishHungerMeterMax += (value) => { hungerMeterMax = value; };
        saveEvent.OnLoadFishHungerCooldown += (value) => { hungerCooldown = value; };
        saveEvent.OnLoadFishAvoidanceForce += (value) => { avoidanceForce = value; };
        saveEvent.OnLoadFishAvoidanceRadius += (value) => { avoidanceRadius = value; };
        saveEvent.OnLoadFishMinBounds += (value) => { minBounds = value; };
        saveEvent.OnLoadFishMaxBounds += (value) => { maxBounds = value; };
    }

    void AddDataFish(FishData newFish)
    {
        fishData.Add(newFish);
    }

    void RemoveDataFish(FishData newFishData)
    {
        Debug.Log($"RemoveFishData {newFishData.fishName}");
        for (int i = fishList.Count - 1; i >= 0; i--)
        {
            if (fishList[i].FishData == newFishData)
            {
                GameObject objToDestroy = fishList[i].gameObject;
                fishList.RemoveAt(i);
                Destroy(objToDestroy);
            }
        }

        for (int i = fishData.Count - 1; i >= 0; i--)
        {
            if (fishData[i] == newFishData)
            {
                fishData.RemoveAt(i);
            }
        }
    }
}

[System.Serializable]
public class SpecsFishByType
{
    public string typeName;
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
}