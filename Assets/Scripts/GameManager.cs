using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int currentLevelIndex = 0;
    [HideInInspector] public GameStatus currentGameStatus = GameStatus.Playing;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public enum GameStatus
    {
        Playing,
        Paused,
        Failed,
        Success
    }
}
