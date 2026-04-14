using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    [SerializeField] private Action<float> onChangeSFXVolume;
    [SerializeField] private Action<bool> onDemo;
    [SerializeField] private Action<Fish> onScaringFish;
    [SerializeField] private Action<Trash> onRemoveTrash;
    [SerializeField] private Action<Vector2> onFeeding;
    public Action<float> OnChangeSFXVolume { get { return onChangeSFXVolume; } set { onChangeSFXVolume = value; } }
    public Action<bool> OnDemo { get { return onDemo; } set { onDemo = value; } }
    public Action<Fish> OnScaringFish { get { return onScaringFish; } set { onScaringFish = value; } }
    public Action<Trash> OnRemoveTrash { get { return onRemoveTrash; } set { onRemoveTrash = value; } }
    public Action<Vector2> OnFeeding { get { return onFeeding; } set { onFeeding = value; } }

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

    public void Feeding(Vector2 mousePos)
    {
        onFeeding?.Invoke(mousePos);
    }
}