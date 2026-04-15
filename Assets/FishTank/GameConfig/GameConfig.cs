using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameConfig
{
    public string gameTitle;

    [Header("Fish")]
    public float fishSpawnerCooldown;
    public Vector2 fishSpawnerMinRange;
    public Vector2 fishSpawnerMaxRange;
    public List<SpecsFishByType> specsFishByType;
    public float fishDetectionRadius;
    public float fishHungerMeterMax;
    public float fishHungerCooldown;
    public float fishAvoidanceForce;
    public float fishAvoidanceRadius;
    public Vector2 fishMinBounds;
    public Vector2 fishMaxBounds;

    [Header("Trash")]
    public float trashSpawnerCooldown;
    public Vector2 trashSpawnerMinRange;
    public Vector2 trashSpawnerMaxRange;
    public List<SpecsTrashByType> specsTrashByType;
    public float trashAvoidanceForce = 5f;
    public float trashAvoidanceRadius = 1.2f;
    public Vector2 trashMinBounds;
    public Vector2 trashMaxBounds;
}