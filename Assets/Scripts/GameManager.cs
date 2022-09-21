using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int CurrentLevelIndex { get; private set; }
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
        CurrentLevelIndex++;
    }

    public enum GameStatus
    {
        Playing,
        Paused,
        Failed,
        Success
    }
}
