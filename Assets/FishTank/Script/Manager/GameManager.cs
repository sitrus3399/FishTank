using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameType gameType;
}

[System.Serializable]
public enum GameType
{
    Play,
    Pause
}