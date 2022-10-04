using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int CurrentLevelIndex { get; private set; } = 0;
    public GameStatus CurrentGameStatus { get; set; }

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

        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        EventManager.LevelSuccesEvent += IncreaseLevelIndex;
    }

    private void OnDisable()
    {
        EventManager.LevelSuccesEvent -= IncreaseLevelIndex;
    }

    private void IncreaseLevelIndex()
    {
        if (CurrentLevelIndex + 1 >= LevelManager.Instance.levelCount)
        {
            CurrentLevelIndex = 2;
        }
        else
        {
            CurrentLevelIndex++;
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
