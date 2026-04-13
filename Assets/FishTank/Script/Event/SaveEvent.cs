using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveEvent", menuName = "ScriptableObjects/SaveEvent", order = 1)]
public class SaveEvent : ScriptableObject
{
    [SerializeField] private Action<float> onSaveSFXVolume;
    [SerializeField] private Action<float> onLoadSFXVolume;
    
    [SerializeField] private Action<float> onSaveMoney;
    [SerializeField] private Action<float> onLoadMoney;

    public Action<float> OnSaveSFXVolume { get { return onSaveSFXVolume; } set { onSaveSFXVolume = value; } }
    public Action<float> OnLoadSFXVolume { get { return onLoadSFXVolume; } set { onLoadSFXVolume = value; } }
    public Action<float> OnSaveMoney { get { return onSaveMoney; } set { onSaveMoney = value; } }
    public Action<float> OnLoadMoney { get { return onLoadMoney; } set { onLoadMoney = value; } }

    public void SaveSFXVolume(float value)
    {
        onSaveSFXVolume?.Invoke(value);
    }

    public void LoadSFXVolume(float value)
    {
        onLoadSFXVolume?.Invoke(value);
    }
    
    public void SaveMoney(float value)
    {
        onSaveSFXVolume?.Invoke(value);
    }

    public void LoadMoney(float value)
    {
        onLoadSFXVolume?.Invoke(value);
    }
}