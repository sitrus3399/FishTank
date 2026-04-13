using UnityEngine;

[CreateAssetMenu(fileName = "TrashData", menuName = "ScriptableObjects/TrashData", order = 1)]
public class TrashData : ScriptableObject
{
    public string trashName;
    public string trashType;
    public Sprite trashSprite;
}