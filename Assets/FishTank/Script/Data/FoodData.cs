using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "ScriptableObjects/FoodData", order = 1)]
public class FoodData : ScriptableObject
{
    public string foodName;
    public Sprite foodSprite;
    public float foodValue;
}