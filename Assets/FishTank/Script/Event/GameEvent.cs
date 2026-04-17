using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    [SerializeField] private Action<float> onChangeSFXVolume;
    [SerializeField] private Action<bool> onDemo;
    [SerializeField] private Action<Fish> onScaringFish;
    [SerializeField] private Action<Trash> onRemoveTrash;

    [SerializeField] private Action<TrashData> onRemoveTrashData;
    [SerializeField] private Action<FishData> onRemoveFishData;

    [SerializeField] private Action<Vector2> onFeeding;

    [SerializeField] private Action<FishData> onAddFish;
    [SerializeField] private Action<TrashData> onAddTrash;
    public Action<float> OnChangeSFXVolume { get { return onChangeSFXVolume; } set { onChangeSFXVolume = value; } }
    public Action<bool> OnDemo { get { return onDemo; } set { onDemo = value; } }
    public Action<Fish> OnScaringFish { get { return onScaringFish; } set { onScaringFish = value; } }
    public Action<Trash> OnRemoveTrash { get { return onRemoveTrash; } set { onRemoveTrash = value; } }

    public Action<TrashData> OnRemoveTrashData { get { return onRemoveTrashData; } set { onRemoveTrashData = value; } }
    public Action<FishData> OnRemoveFishData { get { return onRemoveFishData; } set { onRemoveFishData = value; } }

    public Action<Vector2> OnFeeding { get { return onFeeding; } set { onFeeding = value; } }

    public Action<FishData> OnAddFish { get { return onAddFish; } set { onAddFish = value; } }
    public Action<TrashData> OnAddTrash { get { return onAddTrash; } set { onAddTrash = value; } }

    public void ChangeSFXVolume(float value)
    {
        onChangeSFXVolume?.Invoke(value);
    }

    public void Demo(bool value)
    {
        onDemo?.Invoke(value);
    }

    public void ScaringFish(Fish target)
    {
        onScaringFish?.Invoke(target);
    }

    public void RemoveTrash(Trash target)
    {
        onRemoveTrash?.Invoke(target);
    }
    
    public void RemoveTrashData(TrashData target)
    {
        onRemoveTrashData?.Invoke(target);
    }

    public void RemoveFishData(FishData target)
    {
        onRemoveFishData?.Invoke(target);
    }

    public void Feeding(Vector2 mousePos)
    {
        onFeeding?.Invoke(mousePos);
    }

    public void AddFish(FishData data)
    {
        onAddFish?.Invoke(data);
    }

    public void AddTrash(TrashData data)
    {
        onAddTrash?.Invoke(data);
    }
}