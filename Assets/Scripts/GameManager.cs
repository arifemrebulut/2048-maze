using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int currentLevelIndex = 0;
    [HideInInspector] public GameStatus currentGameStatus = GameStatus.Playing;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
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
