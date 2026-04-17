using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveEvent", menuName = "ScriptableObjects/SaveEvent", order = 1)]
public class SaveEvent : ScriptableObject
{
    [SerializeField] private Action<float> onSaveSFXVolume;

    [Header("Fish")]
    [SerializeField] private Action<float> onLoadFishSpawnerCooldown;
    [SerializeField] private Action<Vector2> onLoadFishSpawnerMinRange;
    [SerializeField] private Action<Vector2> onLoadFishSpawnerMaxRange;
    [SerializeField] private Action<List<SpecsFishByType>> onLoadSpecsFishByType;
    [SerializeField] private Action<float> onLoadFishDetectionRadius;
    [SerializeField] private Action<float> onLoadFishHungerMeterMax;
    [SerializeField] private Action<float> onLoadFishHungerCooldown;
    [SerializeField] private Action<float> onLoadFishAvoidanceForce;
    [SerializeField] private Action<float> onLoadFishAvoidanceRadius;
    [SerializeField] private Action<Vector2> onLoadFishMinBounds;
    [SerializeField] private Action<Vector2> onLoadFishMaxBounds;

    [Header("Trash")]
    [SerializeField] private Action<float> onLoadTrashSpawnerCooldown;
    [SerializeField] private Action<Vector2> onLoadTrashSpawnerMinRange;
    [SerializeField] private Action<Vector2> onLoadTrashSpawnerMaxRange;
    [SerializeField] private Action<List<SpecsTrashByType>> onLoadSpecsTrashByType;
    [SerializeField] private Action<float> onLoadTrashAvoidanceForce;
    [SerializeField] private Action<float> onLoadTrashAvoidanceRadius;
    [SerializeField] private Action<Vector2> onLoadTrashMinBounds;
    [SerializeField] private Action<Vector2> onLoadTrashMaxBounds;

    public Action<float> OnSaveSFXVolume { get { return onSaveSFXVolume; } set { onSaveSFXVolume = value; } }

    [Header("Fish")]
    public Action<float> OnLoadFishSpawnerCooldown { get { return onLoadFishSpawnerCooldown; } set { onLoadFishSpawnerCooldown = value; } }
    public Action<Vector2> OnLoadFishSpawnerMinRange { get { return onLoadFishSpawnerMinRange; } set { onLoadFishSpawnerMinRange = value; } }
    public Action<Vector2> OnLoadFishSpawnerMaxRange { get { return onLoadFishSpawnerMaxRange; } set { onLoadFishSpawnerMaxRange = value; } }
    public Action<List<SpecsFishByType>> OnLoadSpecsFishByType { get { return onLoadSpecsFishByType; } set { onLoadSpecsFishByType = value; } }
    public Action<float> OnLoadFishDetectionRadius { get { return onLoadFishDetectionRadius; } set { onLoadFishDetectionRadius = value; } }
    public Action<float> OnLoadFishHungerMeterMax { get { return onLoadFishHungerMeterMax; } set { onLoadFishHungerMeterMax = value; } }
    public Action<float> OnLoadFishHungerCooldown { get { return onLoadFishHungerCooldown; } set { onLoadFishHungerCooldown = value; } }
    public Action<float> OnLoadFishAvoidanceForce { get { return onLoadFishAvoidanceForce; } set { onLoadFishAvoidanceForce = value; } }
    public Action<float> OnLoadFishAvoidanceRadius { get { return onLoadFishAvoidanceRadius; } set { onLoadFishAvoidanceRadius = value; } }
    public Action<Vector2> OnLoadFishMinBounds { get { return onLoadFishMinBounds; } set { onLoadFishMinBounds = value; } }
    public Action<Vector2> OnLoadFishMaxBounds { get { return onLoadFishMaxBounds; } set { onLoadFishMaxBounds = value; } }


    [Header("Trash")]
    public Action<float> OnLoadTrashSpawnerCooldown { get { return onLoadTrashSpawnerCooldown; } set { onLoadTrashSpawnerCooldown = value; } }
    public Action<Vector2> OnLoadTrashSpawnerMinRange { get { return onLoadTrashSpawnerMinRange; } set { onLoadTrashSpawnerMinRange = value; } }
    public Action<Vector2> OnLoadTrashSpawnerMaxRange { get { return onLoadTrashSpawnerMaxRange; } set { onLoadTrashSpawnerMaxRange = value; } }
    public Action<List<SpecsTrashByType>> OnLoadSpecsTrashByType { get { return onLoadSpecsTrashByType; } set { onLoadSpecsTrashByType = value; } }
    public Action<float> OnLoadTrashAvoidanceForce { get { return onLoadTrashAvoidanceForce; } set { onLoadTrashAvoidanceForce = value; } }
    public Action<float> OnLoadTrashAvoidanceRadius { get { return onLoadTrashAvoidanceRadius; } set { onLoadTrashAvoidanceRadius = value; } }
    public Action<Vector2> OnLoadTrashMinBounds { get { return onLoadTrashMinBounds; } set { onLoadTrashMinBounds = value; } }
    public Action<Vector2> OnLoadTrashMaxBounds { get { return onLoadTrashMaxBounds; } set { onLoadTrashMaxBounds = value; } }

    public void SaveSFXVolume(float value)
    {
        onSaveSFXVolume?.Invoke(value);
    }

    public void LoadFishDataManager(List<SpecsFishByType> specsFishByType, float fishDetectionRadius, float fishHungerMeterMax, float fishHungerCooldown, float fishAvoidanceForce, float fishAvoidanceRadius, Vector2 fishMinBounds, Vector2 fishMaxBounds)
    {
        onLoadSpecsFishByType?.Invoke(specsFishByType);
        onLoadFishDetectionRadius?.Invoke(fishDetectionRadius);
        onLoadFishHungerMeterMax?.Invoke(fishHungerMeterMax);
        onLoadFishHungerCooldown?.Invoke(fishHungerCooldown);
        onLoadFishAvoidanceForce?.Invoke(fishAvoidanceForce);
        onLoadFishAvoidanceRadius?.Invoke(fishAvoidanceRadius);
        onLoadFishMinBounds?.Invoke(fishMinBounds);
        onLoadFishMaxBounds?.Invoke(fishMaxBounds);
    }

    public void LoadFishDataSpawner(float fishSpawnerCooldown, Vector2 fishSpawnerMinRange, Vector2 fishSpawnerMaxRange)
    {
        onLoadFishSpawnerCooldown?.Invoke(fishSpawnerCooldown);
        onLoadFishSpawnerMinRange?.Invoke(fishSpawnerMinRange);
        onLoadFishSpawnerMaxRange?.Invoke(fishSpawnerMaxRange);
    }

    public void LoadTrashDataManager(List<SpecsTrashByType> specsTrashByType, float trashAvoidanceForce, float trashAvoidanceRadius, Vector2 trashMinBounds, Vector2 trashMaxBounds)
    {
        onLoadSpecsTrashByType?.Invoke(specsTrashByType);
        onLoadTrashAvoidanceForce?.Invoke(trashAvoidanceForce);
        onLoadTrashAvoidanceRadius?.Invoke(trashAvoidanceRadius);
        onLoadTrashMinBounds?.Invoke(trashMinBounds);
        onLoadTrashMaxBounds?.Invoke(trashMaxBounds);
    }

    public void LoadTrashDataSpawner(float trashSpawnerCooldown, Vector2 trashSpawnerMinRange, Vector2 trashSpawnerMaxRange)
    {
        onLoadTrashSpawnerCooldown?.Invoke(trashSpawnerCooldown);
        onLoadTrashSpawnerMinRange?.Invoke(trashSpawnerMinRange);
        OnLoadTrashSpawnerMaxRange?.Invoke(trashSpawnerMaxRange);
    }
}