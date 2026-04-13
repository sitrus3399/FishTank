using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIEvent", menuName = "ScriptableObjects/UIEvent", order = 1)]
public class UIEvent : ScriptableObject
{
    [SerializeField] private Action onClosePanel;
    [SerializeField] private Action onQuitGame;
    [SerializeField] private Action onMainPanel;
    [SerializeField] private Action onSettingPanel;

    [SerializeField] private Action<bool> setActiveTopBar;

    public Action OnClosePanel { get { return onClosePanel; } set { onClosePanel += value; } }
    public Action OnQuitGame { get { return onQuitGame; } set { onQuitGame += value; } }
    public Action OnMainPanel { get { return onMainPanel; } set { onMainPanel += value; } }
    public Action OnSettingPanel { get { return onSettingPanel; } set { onSettingPanel += value; } }
    public Action<bool> SetActiveTopBar { get { return setActiveTopBar; } set { setActiveTopBar += value; } }

    public void ClosePanel()
    {
        onClosePanel?.Invoke();
    }

    public void QuitGame()
    {
        onQuitGame?.Invoke();
    }
    public void MainPanel()
    {
        onMainPanel?.Invoke();
    }

    public void SettingPanel()
    {
        onSettingPanel?.Invoke();
    }

    public void ActiveTopBar(bool value)
    {
        setActiveTopBar?.Invoke(value);
    }
}