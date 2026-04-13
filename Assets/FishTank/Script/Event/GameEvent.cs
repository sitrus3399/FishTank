using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    [SerializeField] private Action<float> onChangeSFXVolume;
    [SerializeField] private Action<float> onUpdateMoney;
    [SerializeField] private Action<float> onDecreaseMoney;

    [SerializeField] private Action<string> onOpenNotif;

    [SerializeField] private Action<bool> onDemo;

    [SerializeField] private Action<Fish> gotFish;

    public Action<float> OnChangeSFXVolume { get { return onChangeSFXVolume; } set { onChangeSFXVolume = value; } }
    public Action<float> OnUpdateMoney { get { return onUpdateMoney; } set { onUpdateMoney = value; } }
    public Action<float> OnDecreaseMoney { get { return onDecreaseMoney; } set { onDecreaseMoney = value; } }

    public Action<string> OnOpenNotif { get { return onOpenNotif; } set { onOpenNotif = value; } }

    public Action<bool> OnDemo { get { return onDemo; } set { onDemo = value; } }

    public Action<Fish> GotFish { get { return gotFish; } set { gotFish = value; } }

    public void ChangeSFXVolume(float value)
    {
        onChangeSFXVolume?.Invoke(value);
    }

    public void Demo(bool value)
    {
        onDemo?.Invoke(value);
    }

    public void UpdateMoney(float value)
    {
        onUpdateMoney?.Invoke(value);
    }

    public void DecreaseMoney(float value)
    {
        onDecreaseMoney?.Invoke(value);
    }

    public void OpenNotif(string notif)
    {
        OnOpenNotif?.Invoke(notif);
    }

    public void Fish(Fish newFish)
    {
        gotFish?.Invoke(newFish);
    }
}